using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Config;
using Intersect.Enums;
using Intersect.Framework.Core.Config;
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

    // Resource Number
    public ResourceBase Base;
    public JobType Jobs  { get; set; } // Agrega esta propiedad para el tipo de trabajo (job)
    public int ExperienceAmount { get; set; } // Agrega esta propiedad para la cantidad de experiencia (xp)
    //Respawn
    public long RespawnTime = 0;
    public int Level { get; set; }
  
    [NotMapped]
    public int BonusDrop { get; set; }
    public Resource(ResourceBase resource) : base()
    {
        Base = resource;
        Name = resource.Name;
        Sprite = resource.Initial.Graphic;
        SetMaxVital(
            Vital.Health,
            Randomization.Next(
                Math.Min(1, resource.MinHp), Math.Max(resource.MaxHp, Math.Min(1, resource.MinHp)) + 1
            )
        );

        RestoreVital(Vital.Health);
        Passable = resource.WalkableBefore;
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
        
        Sprite = Base.Exhausted.Graphic;
        Passable = Base.WalkableAfter;
        Dead = true;

        if (dropItems)
        {
            SpawnResourceItems(killer);
            if (Base.AnimationId != Guid.Empty)
            {
                PacketSender.SendAnimationToProximity(
                    Base.AnimationId, -1, Guid.Empty, MapId, X, Y, Direction.Up, MapInstanceId
                );
            }
        }

        PacketSender.SendEntityDataToProximity(this);
        PacketSender.SendEntityPositionToAll(this);
    }

    public void Spawn()
    {
        Sprite = Base.Initial.Graphic;
        if (Base.MaxHp < Base.MinHp)
        {
            Base.MaxHp = Base.MinHp;
        }

        SetMaxVital(Vital.Health, Randomization.Next(Base.MinHp, Base.MaxHp + 1));
        RestoreVital(Vital.Health);
        Passable = Base.WalkableBefore;
        Items.Clear();
        Jobs = Base.Jobs; // Asigna el tipo de trabajo desde la configuración base
        ExperienceAmount = Base.ExperienceAmount; // Asigna la cantidad de experiencia desde la configuración base
        //Give Resource Drops
        var itemSlot = 0;
        foreach (var drop in Base.Drops)
        {
            if (Randomization.Next(1, 10001) <= drop.Chance * 100 && ItemBase.Get(drop.ItemId) != null)
            {
                var slot = new InventorySlot(itemSlot);
                slot.Set(new Item(drop.ItemId, Randomization.Next(drop.MinQuantity, drop.Quantity + 1+ BonusDrop)));
                Items.Add(slot);
                itemSlot++;
            }
        }

        Dead = false;
        PacketSender.SendEntityDataToProximity(this);
        PacketSender.SendEntityPositionToAll(this);
    }

    public void SpawnResourceItems(Entity killer)
    {
        //Find tile to spawn items
        var tiles = new List<TileHelper>();
        for (var x = X - 1; x <= X + 1; x++)
        {
            for (var y = Y - 1; y <= Y + 1; y++)
            {
                var tileHelper = new TileHelper(MapId, x, y);
                if (tileHelper.TryFix())
                {
                    //Tile is valid.. let's see if its open
                    var mapId = tileHelper.GetMapId();
                    if (MapController.TryGetInstanceFromMap(mapId, MapInstanceId, out var mapInstance))
                    {
                        if (!mapInstance.TileBlocked(tileHelper.GetX(), tileHelper.GetY()))
                        {
                            tiles.Add(tileHelper);
                        }
                        else
                        {
                            if (killer.MapId == tileHelper.GetMapId() &&
                                killer.X == tileHelper.GetX() &&
                                killer.Y == tileHelper.GetY())
                            {
                                tiles.Add(tileHelper);
                            }
                        }
                    }
                }
            }
        }

        if (tiles.Count > 0)
        {
            TileHelper selectedTile = null;

            //Prefer the players tile, otherwise choose randomly
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].GetMapId() == killer.MapId &&
                    tiles[i].GetX() == killer.X &&
                    tiles[i].GetY() == killer.Y)
                {
                    selectedTile = tiles[i];
                }
            }

            if (selectedTile == null)
            {
                selectedTile = tiles[Randomization.Next(0, tiles.Count)];
            }
            
            var itemSource = AsItemSource();

            // Drop items
            foreach (var item in Items)
            {
                if (ItemBase.Get(item.ItemId) != null)
                {
                    var mapId = selectedTile.GetMapId();
                    if (MapController.TryGetInstanceFromMap(mapId, MapInstanceId, out var mapInstance))
                    {
                        mapInstance.SpawnItem(itemSource, selectedTile.GetX(), selectedTile.GetY(), item, item.Quantity, killer.Id);
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
            Id = this.Base.Id
        };
    }

    public override void ProcessRegen()
    {
        //For now give npcs/resources 10% health back every regen tick... in the future we should put per-npc and per-resource regen settings into their respective editors.
        if (!IsDead())
        {
            if (Base == null)
            {
                return;
            }
            
            var vital = Vital.Health;

            var vitalId = (int) vital;
            var vitalValue = GetVital(vital);
            var maxVitalValue = GetMaxVital(vital);
            if (vitalValue < maxVitalValue)
            {
                var vitalRegenRate = Base.VitalRegen / 100f;
                var regenValue = (long) Math.Max(1, maxVitalValue * vitalRegenRate) *
                                 Math.Abs(Math.Sign(vitalRegenRate));

                AddVital(vital, regenValue);
            }
        }
    }

    public override bool IsPassable()
    {
        return IsDead() & Base.WalkableAfter || !IsDead() && Base.WalkableBefore;
    }

    public override EntityPacket EntityPacket(EntityPacket packet = null, Player forPlayer = null)
    {
        if (packet == null)
        {
            packet = new ResourceEntityPacket();
        }

        packet = base.EntityPacket(packet, forPlayer);

        var pkt = (ResourceEntityPacket) packet;
        pkt.ResourceId = Base.Id;
        pkt.IsDead = IsDead();

        return pkt;
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
