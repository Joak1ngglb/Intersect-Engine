using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Config;
using Intersect.Enums;
using Intersect.Framework.Core.Config;
using Intersect.Framework.Core.GameObjects.Items;
using Intersect.Framework.Core.GameObjects.Resources;
using Intersect.Framework.Reflection;
using Intersect.GameObjects;
using Intersect.Network.Packets.Server;
using Intersect.Server.Database;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Server.Framework.Entities;
using Intersect.Server.Framework.Items;
using Intersect.Server.Localization;
using Intersect.Server.Maps;
using Intersect.Server.Networking;
using Intersect.Utilities;

namespace Intersect.Server.Entities;

public partial class Resource : Entity
{
    public readonly ResourceDescriptor Descriptor;

    // Resource Number
    public ResourceBase Base;
    public JobType Jobs  { get; set; } // Agrega esta propiedad para el tipo de trabajo (job)
    public int ExperienceAmount { get; set; } // Agrega esta propiedad para la cantidad de experiencia (xp)
    //Respawn
    public long RespawnTime = 0;
    public int Level { get; set; }
  
    [NotMapped]
    public int BonusDrop { get; set; }
    public Resource(ResourceDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        Descriptor = descriptor;
        Name = descriptor.Name;
        Sprite = descriptor.Initial.Graphic;
        SetMaxVital(
            Vital.Health,
            Randomization.Next(Math.Min(1, descriptor.MinHp), Math.Max(descriptor.MaxHp, Math.Min(1, descriptor.MinHp)) + 1)
        );

        RestoreVital(Vital.Health);
        Passable = descriptor.WalkableBefore;
        HideName = true;
        Jobs= JobType.JobCount; // Establece el tipo de trabajo predeterminado
        ExperienceAmount = 0; // Establece la cantidad de experiencia predeterminada
        Level = resource.Level;
    }

    public void Destroy(bool dropItems = false, Entity killer = null)
    {
        lock (EntityLock)
        {
            Die(dropItems, killer);
        }

        PacketSender.SendEntityDie(this);
        PacketSender.SendEntityLeave(this);
    }

    public override void Die(bool dropItems = true, Entity killer = null)
    {
        if (killer is Player player)
        {
            if (ExperienceAmount > 0)
            {
                // Verificar si el recurso tiene un trabajo asociado
                if (Base.Jobs != JobType.None)
                {
                    // Verificar si el jugador tiene inicializado el trabajo
                    if (player.Jobs.TryGetValue(Base.Jobs, out var playerJob))
                    {
                        // Otorgar experiencia al trabajo
                        player.GiveJobExperience(Base.Jobs, ExperienceAmount);
                        CalculateBonusDrop(playerJob.JobLevel, Level);
                        // Enviar mensaje de experiencia ganada
                        PacketSender.SendChatMsg(
                            player,
                            string.Format(
                                Strings.CraftingNamespace.GetJobExperienceMessage(Base.Jobs, ExperienceAmount)
                            ),
                            ChatMessageType.Experience,
                            CustomColors.Chat.PlayerMsg
                        );
                    }
                    else
                    {
                        // Trabajo desconocido en el jugador
                        PacketSender.SendChatMsg(
                            player,
                            string.Format(Strings.Crafting.UnknownJobType, Base.Jobs),
                            ChatMessageType.Error,
                            Color.Orange
                        );
                    }
                }
                else
                {
                    // Si no hay un trabajo asignado al recurso
                    PacketSender.SendChatMsg(
                        player,
                        "No Jobs Assigned",
                        ChatMessageType.Error,
                        Color.Orange
                    );
                }
            }
        }


        lock (EntityLock)
        {
            base.Die(false, killer);
        }

        Sprite = Descriptor.Exhausted.Graphic;
        Passable = Descriptor.WalkableAfter;
        IsDead = true;

        if (dropItems)
        {
            SpawnResourceItems(killer);
        }

        PacketSender.SendEntityDataToProximity(this);
        PacketSender.SendEntityPositionToAll(this);
    }

    public void Spawn()
    {
        Sprite = Descriptor.Initial.Graphic;
        var minimumHealth = Descriptor.MinHp;
        var maximumHealth = Descriptor.MaxHp;

        // Ensure the minimum health is at least 1
        minimumHealth = Math.Max(1, minimumHealth);

        // Ensure the maximum health is at least the same as the minimum health
        maximumHealth = Math.Max(maximumHealth, minimumHealth);

        var randomizedHealth = Randomization.Next(minimumHealth, maximumHealth + 1);

        SetMaxVital(Vital.Health, randomizedHealth);
        RestoreVital(Vital.Health);

        Passable = Descriptor.WalkableBefore;
        Items.Clear();
        Jobs = Base.Jobs; // Asigna el tipo de trabajo desde la configuración base
        ExperienceAmount = Base.ExperienceAmount; // Asigna la cantidad de experiencia desde la configuración base
        //Give Resource Drops
        var itemSlot = 0;
        foreach (var drop in Descriptor.Drops)
        {
            var roll = Randomization.Next(1, 10001);
            var maximumRoll = drop.Chance * 100;

            if (roll > maximumRoll || !ItemDescriptor.TryGet(drop.ItemId, out _))
            {
                slot.Set(new Item(drop.ItemId, Randomization.Next(drop.MinQuantity, drop.Quantity + 1) + BonusDrop));
            }

            var slot = new InventorySlot(itemSlot);
            var dropQuantity = Randomization.Next(drop.MinQuantity, drop.MaxQuantity + 1);
            var dropInstance = new Item(drop.ItemId, dropQuantity);
            slot.Set(dropInstance);

            Items.Add(slot);
            itemSlot++;
        }

        IsDead = false;
        PacketSender.SendEntityDataToProximity(this);
        PacketSender.SendEntityPositionToAll(this);
    }

    private void SpawnResourceItems(Entity killer)
    {
        //Find tile to spawn items
        var tiles = new List<TileHelper>();
        for (var x = X - 1; x <= X + 1; x++)
        {
            for (var y = Y - 1; y <= Y + 1; y++)
            {
                var tileHelper = new TileHelper(MapId, x, y);
                if (!tileHelper.TryFix())
                {
                    continue;
                }

                // Tile is valid, let's see if its open
                var mapId = tileHelper.GetMapId();
                if (!MapController.TryGetInstanceFromMap(mapId, MapInstanceId, out var mapInstance))
                {
                    continue;
                }

                var tileHelperX = tileHelper.GetX();
                var tileHelperY = tileHelper.GetY();
                if (mapInstance.TileBlocked(tileHelperX, tileHelperY))
                {
                    if (killer.MapId == tileHelper.GetMapId() && killer.X == tileHelperX && killer.Y == tileHelperY)
                    {
                        tiles.Add(tileHelper);
                    }
                }
                else
                {
                    tiles.Add(tileHelper);
                }
            }
        }

        if (tiles.Count > 0)
        {
            TileHelper selectedTile = null;

            // Prefer the players tile, otherwise choose randomly
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var tileHelper in tiles)
            {
                if (tileHelper.GetMapId() == killer.MapId &&
                    tileHelper.GetX() == killer.X &&
                    tileHelper.GetY() == killer.Y)
                {
                    selectedTile = tileHelper;
                }
            }

            selectedTile ??= tiles[Randomization.Next(0, tiles.Count)];

            var itemSource = AsItemSource();

            // Drop items
            foreach (var item in Items)
            {
                if (ItemDescriptor.Get(item.ItemId) != null)
                {
                    var mapId = selectedTile.GetMapId();
                    if (MapController.TryGetInstanceFromMap(mapId, MapInstanceId, out var mapInstance))
                    {
                        mapInstance.SpawnItem(
                            itemSource,
                            selectedTile.GetX(),
                            selectedTile.GetY(),
                            item,
                            item.Quantity,
                            killer.Id
                        );
                    }
                }
            }
        }

        Items.Clear();
    }

    protected override EntityItemSource? AsItemSource()
    {
        return new EntityItemSource
        {
            EntityType = GetEntityType(),
            EntityReference = new WeakReference<IEntity>(this),
            Id = Descriptor.Id,
        };
    }

    public override void ProcessRegen()
    {
        // For now give NPCs/resources 10% health back every regen tick... in the future we should put per-npc and per-resource regen settings into their respective editors.
        if (IsDead)
        {
            return;
        }

        var vitalValue = GetVital(Vital.Health);
        var maxVitalValue = GetMaxVital(Vital.Health);
        if (vitalValue >= maxVitalValue)
        {
            return;
        }

        var vitalRegenRate = Descriptor.VitalRegen / 100f;
        var regenValue = (long)Math.Max(1, maxVitalValue * vitalRegenRate);

        AddVital(Vital.Health, regenValue);
    }

    public override bool IsPassable()
    {
        return IsDead & Descriptor.WalkableAfter || (!IsDead && Descriptor.WalkableBefore);
    }

    public override EntityPacket EntityPacket(EntityPacket? packet = null, Player? forPlayer = null)
    {
        packet ??= new ResourceEntityPacket();

        packet = base.EntityPacket(packet, forPlayer);

        if (packet is not ResourceEntityPacket resourceEntityPacket)
        {
            throw new InvalidOperationException(
                $"Invalid packet type '{packet.GetType().GetName(qualified: true)}', expected '{typeof(ResourceEntityPacket).GetName(qualified: true)}"
            );
        }

        resourceEntityPacket.ResourceId = Descriptor.Id;
        resourceEntityPacket.IsDead = IsDead;

        return resourceEntityPacket;
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Resource;
    }
    public void CalculateBonusDrop(int resourceLevel, int jobLevel)
    {
        // Validar que ambos niveles sean positivos
        if (jobLevel < 0 || resourceLevel < 0)
        {
            Console.WriteLine("Error: jobLevel y resourceLevel deben ser valores positivos.");
            return;
        }

        // Calcular la diferencia entre el nivel del trabajo y el nivel del recurso
        int levelDifference = jobLevel - resourceLevel;

        // Determinar el bonus de drop basado en la diferencia
        BonusDrop = Math.Max(0, levelDifference / 10);

    }

}
