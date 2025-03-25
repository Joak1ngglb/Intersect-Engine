using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Core;
using Intersect.Framework.Core.GameObjects.Items;
using Intersect.GameObjects.Ranges;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;
using Microsoft.Extensions.Logging;

namespace Intersect.Client.Interface.Game.DescriptionWindows;

public partial class ItemDescriptionWindow : DescriptionWindowBase
{
    protected ItemDescriptor mItem;

    protected int mAmount;

    protected ItemProperties? mItemProperties;

    protected string mTitleOverride;

    protected string mValueLabel;

    protected SpellDescriptionWindow? mSpellDescWindow;

    public ItemDescriptionWindow(
        ItemDescriptor item,
        int amount,
        int x,
        int y,
        ItemProperties? itemProperties,
        string titleOverride = "",
        string valueLabel = ""
    ) : base(Interface.GameUi.GameCanvas, "DescriptionWindow")
    {
        mItem = item;
        mAmount = amount;
        mItemProperties = itemProperties;
        mTitleOverride = titleOverride;
        mValueLabel = valueLabel;

        GenerateComponents();
        SetupDescriptionWindow();
        SetPosition(x, y);

        // If a spell, also display the spell description!
        if (mItem.ItemType == ItemType.Spell)
        {
            mSpellDescWindow = new SpellDescriptionWindow(mItem.SpellId, x, y, mContainer);
        }
    }

    protected void SetupDescriptionWindow()
    {
        if (mItem == null)
        {
            return;
        }

        // Set up our header information.
        SetupHeader();

        // Set up our item limit information.
        SetupItemLimits();

        // if we have a description, set that up.
        if (!string.IsNullOrWhiteSpace(mItem.Description))
        {
            SetupDescription();
        }

        // Set up information depending on the item type.
        switch (mItem.ItemType)
        {
            case ItemType.Equipment:
                SetupEquipmentInfo();
                break;

            case ItemType.Consumable:
                SetupConsumableInfo();
                break;

            case ItemType.Spell:
                SetupSpellInfo();
                break;

            case ItemType.Bag:
                SetupBagInfo();
                break;
        }

        // Set up additional information such as amounts and shop values.
        SetupExtraInfo();

        // Resize the container, correct the display and position our window.
        FinalizeWindow();
    }

    protected void SetupHeader()
    {
        // Crear el encabezado de la descripción
        var header = AddHeader();

        // Configurar el icono del ítem si está disponible
        var tex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, mItem.Icon);
        if (tex != null)
        {
            header.SetIcon(tex, mItem.Color);
        }

        // Obtener el color de rareza del ítem
        if (CustomColors.Items.Rarities.TryGetValue(mItem.Rarity, out var rarityColor))
        {
            // Aplicar el color de fondo al encabezado de la descripción
            header.SetBackgroundColor(rarityColor);
        }
        else
        {
            header.SetBackgroundColor(Color.White); // Color por defecto si no hay rareza
        }

        // Establecer el título con el color de la rareza
        var name = !string.IsNullOrWhiteSpace(mTitleOverride) ? mTitleOverride : mItem.Name;
        header.SetTitle(name, rarityColor ?? Color.White);

        // Configurar la descripción del tipo de ítem
        Strings.ItemDescription.ItemTypes.TryGetValue((int)mItem.ItemType, out var typeDesc);
        if (mItem.ItemType == ItemType.Equipment)
        {
            var equipSlot = Options.Instance.Equipment.Slots[mItem.EquipmentSlot];
            var extraInfo = equipSlot;
            if (mItem.EquipmentSlot == Options.Instance.Equipment.WeaponSlot && mItem.TwoHanded)
            {
                extraInfo = $"{Strings.ItemDescription.TwoHand} {equipSlot}";
            }
            header.SetSubtitle($"{typeDesc} - {extraInfo}", Color.White);
        }
        else
        {
            header.SetSubtitle(typeDesc, Color.White);
        }

        // Configurar la etiqueta de rareza en la descripción
        try
        {
            if (Options.Instance.Items.TryGetRarityName(mItem.Rarity, out var rarityName))
            {
                _ = Strings.ItemDescription.Rarity.TryGetValue(rarityName, out var rarityLabel);
                header.SetDescription(rarityLabel, rarityColor ?? Color.White);
            }
        }
        catch (Exception exception)
        {
            ApplicationContext.Context.Value?.Logger.LogError(
                exception,
                "Error setting rarity description for rarity {Rarity}",
                mItem.Rarity
            );
            throw;
        }

        header.SizeToChildren(true, false);
    }

    protected void SetupItemLimits()
    {
        // Gather up what limitations apply to this item.
        var limits = new List<string>();
        if (!mItem.CanBank)
        {
            limits.Add(Strings.ItemDescription.Banked);
        }
        if (!mItem.CanGuildBank)
        {
            limits.Add(Strings.ItemDescription.GuildBanked);
        }
        if (!mItem.CanBag)
        {
            limits.Add(Strings.ItemDescription.Bagged);
        }
        if (!mItem.CanTrade)
        {
            limits.Add(Strings.ItemDescription.Traded);
        }
        if (!mItem.CanDrop)
        {
            limits.Add(Strings.ItemDescription.Dropped);
        }
        if (!mItem.CanSell)
        {
            limits.Add(Strings.ItemDescription.Sold);
        }

        // Do we have any limitations? If so, generate a display for it.
        if (limits.Count > 0)
        {
            // Add a divider.
            AddDivider();

            // Add the actual description.
            var description = AddDescription();

            // Commbine our lovely limitations to a single line and display them.
            description.AddText(Strings.ItemDescription.ItemLimits.ToString(string.Join(", ", limits)), Color.White);
        }
    }

    protected void SetupDescription()
    {
        // Add a divider.
        AddDivider();

        // Add the actual description.
        var description = AddDescription();
        description.AddText(Strings.ItemDescription.Description.ToString(mItem.Description), Color.White);
    }
    private int GetStatDifference(int statIndex)
    {
        var slot = mItem.EquipmentSlot;

        // Obtener la estadística base del nuevo ítem
        var newItemStat = mItem.StatsGiven[statIndex];
        if (mItemProperties?.StatModifiers != null)
        {
            newItemStat += mItemProperties.StatModifiers[statIndex];
        }

        // Verificar si hay un ítem equipado en la misma ranura
        if (Globals.Me.MyEquipment[slot] != -1)
        {
            var equippedItem = Globals.Me.Inventory[Globals.Me.MyEquipment[slot]];
            if (equippedItem != null)
            {
                // Obtener las estadísticas del ítem equipado
                var equippedStat = equippedItem.Base.StatsGiven[statIndex];
                if (equippedItem.ItemProperties?.StatModifiers != null)
                {
                    equippedStat += equippedItem.ItemProperties.StatModifiers[statIndex];
                }

                // Devolver la diferencia entre las estadísticas del nuevo y del equipado
                return newItemStat - equippedStat;
            }
            else
            {
                // No hay ítem equipado; devolver la estadística del nuevo ítem
                return newItemStat;
            }
        }
        else
        {
            // No hay ítem en la ranura; devolver la estadística del nuevo ítem
            return newItemStat;
        }
    }


    private int GetBaseDamageDifference()
    {
        var equippedDamage = GetEquippedWeapon()?.Damage ?? 0;
        return mItem.Damage - equippedDamage;
    }

    private ItemBase? GetEquippedWeapon()
    {
        var slot = Options.WeaponIndex;
        return Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base
            : null;
    }
    private int GetVitalDifference(int vitalIndex)
    {
        var equippedVital = GetEquippedVital(vitalIndex);
        var newItemVital = mItem.VitalsGiven[vitalIndex];
        return (int)(newItemVital - equippedVital);
    }

    private int GetEquippedVital(int vitalIndex)
    {
        var slot = mItem.EquipmentSlot;
        var equippedItem = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base
            : null;

        return (int)(equippedItem?.VitalsGiven[vitalIndex] ?? 0);
    }
    private int GetBonusEffectDifference(ItemEffect effectType)
    {
        var equippedEffect = GetEquippedEffect(effectType);
        var newEffect = mItem.Effects.FirstOrDefault(e => e.Type == effectType)?.Percentage ?? 0;
        return newEffect - equippedEffect;
    }

    private int GetEquippedEffect(ItemEffect effectType)
    {
        var slot = mItem.EquipmentSlot;
        var equippedItem = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base
            : null;

        return equippedItem?.Effects.FirstOrDefault(e => e.Type == effectType)?.Percentage ?? 0;
    }
    private int GetVitalRegenDifference(int vitalIndex)
    {
        var equippedRegen = GetEquippedVitalRegen(vitalIndex);
        var newItemRegen = mItem.VitalsRegen[vitalIndex];
        return (int)(newItemRegen - equippedRegen);
    }

    private int GetEquippedVitalRegen(int vitalIndex)
    {
        var slot = mItem.EquipmentSlot;
        var equippedItem = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base
            : null;

        return (int)(equippedItem?.VitalsRegen[vitalIndex] ?? 0);
    }
    private int GetCritChanceDifference()
    {
        var slot = mItem.EquipmentSlot;
        var equippedCritChance = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base?.CritChance ?? 0
            : 0;

        return mItem.CritChance - equippedCritChance;
    }

    private double GetCritMultiplierDifference()
    {
        var slot = mItem.EquipmentSlot;
        var equippedCritMultiplier = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base?.CritMultiplier ?? 0
            : 0;

        return mItem.CritMultiplier - equippedCritMultiplier;
    }
 
    private int GetScalingDifference()
    {
        var slot = mItem.EquipmentSlot;
        var equippedScaling = Globals.Me.MyEquipment[slot] != -1
            ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]].Base?.Scaling ?? 0
            : 0;

        return mItem.Scaling - equippedScaling;
    }


    protected void SetupEquipmentInfo()
    {
        // Add a divider.
        AddDivider();

        // Add a row component.
        var rows = AddRowContainer();

        // Is this a weapon?
        if (mItem.EquipmentSlot == Options.Instance.Equipment.WeaponSlot)
        {
            // Base Damage:
            var damageDiff = GetBaseDamageDifference();
            DisplayKeyValueRowWithDifference(damageDiff, Strings.ItemDescription.BaseDamage, mItem.Damage.ToString(), rows);

            // Damage Type:
            Strings.ItemDescription.DamageTypes.TryGetValue(mItem.DamageType, out var damageType);
            rows.AddKeyValueRow(Strings.ItemDescription.BaseDamageType, damageType);

            if (mItem.Scaling > 0)
            {
                Strings.ItemDescription.Stats.TryGetValue(mItem.ScalingStat, out var stat);
                var scalingDiff = GetScalingDifference();
                DisplayKeyValueRowWithDifference(scalingDiff, Strings.ItemDescription.ScalingStat, stat, rows);

                rows.AddKeyValueRow(Strings.ItemDescription.ScalingPercentage, Strings.ItemDescription.Percentage.ToString(mItem.Scaling));
            }

            // Crit Chance
            if (mItem.CritChance > 0)
            {
                var critChanceDiff = GetCritChanceDifference();
                DisplayKeyValueRowWithDifference(critChanceDiff, Strings.ItemDescription.CritChance, Strings.ItemDescription.Percentage.ToString(mItem.CritChance), rows);

                var critMultiplierDiff = GetCritMultiplierDifference();
                DisplayKeyValueRowWithDifference(critMultiplierDiff, Strings.ItemDescription.CritMultiplier, Strings.ItemDescription.Multiplier.ToString(mItem.CritMultiplier), rows);
            }

            // Attack Speed
            if (mItem.AttackSpeedModifier == 0)
            {
                // Calculate base attack speed manually.
                var speed = Globals.Me.Stat[(int)Stat.Speed];

                // Remove currently equipped weapon stats.. We want to create a fair display!
                var weaponSlot = Globals.Me.MyEquipment[Options.Instance.Equipment.WeaponSlot];
                if (weaponSlot != -1)
                {
                    var randomStats = Globals.Me.Inventory[weaponSlot].ItemProperties.StatModifiers;
                    var weapon = ItemDescriptor.Get(Globals.Me.Inventory[weaponSlot].ItemId);
                    if (weapon != null && randomStats != null)
                    {
                        speed = (int)Math.Round(speed / ((100 + weapon.PercentageStatsGiven[(int)Stat.Speed]) / 100f));
                        speed -= weapon.StatsGiven[(int)Stat.Speed];
                        speed -= randomStats[(int)Stat.Speed];
                    }
                }

                // Add current item's speed stats.
                if (mItemProperties?.StatModifiers != default)
                {
                    speed += mItem.StatsGiven[(int)Stat.Speed];
                    speed += mItemProperties.StatModifiers[(int)Stat.Speed];
                    speed += (int)Math.Floor(speed * (mItem.PercentageStatsGiven[(int)Stat.Speed] / 100f));
                }

                // Display calculated attack speed.
                rows.AddKeyValueRow(Strings.ItemDescription.AttackSpeed, TimeSpan.FromMilliseconds(Globals.Me.CalculateAttackTime(speed)).WithSuffix());

                // Compare with equipped weapon's attack speed.
                var attackSpeedDiff = GetAttackSpeedDifference();
                DisplayKeyValueRowWithDifference(attackSpeedDiff, Strings.ItemDescription.AttackSpeedComparison, TimeSpan.FromMilliseconds(Globals.Me.CalculateAttackTime(speed)).WithSuffix(), rows);
            }
            else if (mItem.AttackSpeedModifier == 1)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.AttackSpeed, TimeSpan.FromMilliseconds(mItem.AttackSpeedValue).WithSuffix());
            }
            else if (mItem.AttackSpeedModifier == 2)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.AttackSpeed, Strings.ItemDescription.Percentage.ToString(mItem.AttackSpeedValue));
            }
        }

        //Blocking options
        if (mItem.EquipmentSlot == Options.Instance.Equipment.ShieldSlot)
        {
            if (mItem.BlockChance > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.BlockChance, Strings.ItemDescription.Percentage.ToString(mItem.BlockChance));
            }

            if (mItem.BlockAmount > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.BlockAmount, Strings.ItemDescription.Percentage.ToString(mItem.BlockAmount));
            }

            if (mItem.BlockAbsorption > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.BlockAbsorption, Strings.ItemDescription.Percentage.ToString(mItem.BlockAbsorption));
            }
        }

       
        // Vitals
        for (var i = 0; i < Enum.GetValues<Vital>().Length; i++)
        {
            var vitalLabel = Strings.ItemDescription.Vitals[i];
            var vitalValue = mItem.VitalsGiven[i];
            var percentageVitalValue = mItem.PercentageVitalsGiven[i];
            var vitalDiff = GetVitalDifference(i);

            if (vitalValue != 0 && percentageVitalValue != 0)
            {
                DisplayKeyValueRowWithDifferenceAndPercent(vitalDiff, percentageVitalValue, vitalLabel, vitalValue.ToString(), rows);
            }
            else if (vitalValue != 0)
            {
                DisplayKeyValueRowWithDifference(vitalDiff, vitalLabel, vitalValue.ToString(), rows);
            }
            else if (percentageVitalValue != 0)
            {
                rows.AddKeyValueRow(vitalLabel, Strings.ItemDescription.Percentage.ToString(percentageVitalValue), CustomColors.ItemDesc.Muted,Color.White);
            }
        }


        // Vitals Regen
        for (var i = 0; i < Enum.GetValues<Vital>().Length; i++)
        {
            var vitalRegenLabel = Strings.ItemDescription.VitalsRegen[i];
            var vitalRegenValue = mItem.VitalsRegen[i];
            var vitalRegenDiff = GetVitalRegenDifference(i);

            if (vitalRegenValue > 0)
            {
                DisplayKeyValueRowWithDifference(vitalRegenDiff, vitalRegenLabel, Strings.ItemDescription.Percentage.ToString(vitalRegenValue), rows);
            }
        }
        // Stats
        var statModifiers = mItemProperties?.StatModifiers;
        for (var statIndex = 0; statIndex < Enum.GetValues<Stat>().Length; statIndex++)
        {
            var stat = (Stat)statIndex;
            var statLabel = Strings.ItemDescription.StatCounts[statIndex];
            ItemRange? rangeForStat = default;
            var percentageGivenForStat = mItem.PercentageStatsGiven[statIndex];
            var statDiff = GetStatDifference(statIndex);

            // Si hay modificadores o el rango de estadísticas es fijo
            if (statModifiers != null || !mItem.TryGetRangeFor(stat, out rangeForStat) || rangeForStat.LowRange == rangeForStat.HighRange)
            {
                var flatValueGivenForStat = mItem.StatsGiven[statIndex];
                if (statModifiers != null)
                {
                    flatValueGivenForStat += statModifiers[statIndex];
                }

                flatValueGivenForStat += rangeForStat?.LowRange ?? 0;

                if (flatValueGivenForStat != 0 && percentageGivenForStat != 0)
                {
                    // Mostrar valores regulares y porcentuales con diferencias y colores
                    DisplayKeyValueRowWithDifferenceAndPercent(
                        statDiff,
                        percentageGivenForStat,
                        statLabel,
                        flatValueGivenForStat.ToString(),
                        rows
                    );
                }
                else if (flatValueGivenForStat != 0)
                {
                    // Mostrar solo valores regulares con diferencias y colores
                    DisplayKeyValueRowWithDifference(
                        statDiff,
                        statLabel,
                        flatValueGivenForStat.ToString(),
                        rows
                    );
                }
                else if (percentageGivenForStat != 0)
                {
                    // Mostrar solo valores porcentuales
                    rows.AddKeyValueRow(
                        statLabel,
                        Strings.ItemDescription.Percentage.ToString(percentageGivenForStat),
                        CustomColors.ItemDesc.Muted,
                        CustomColors.ItemDesc.Muted
                    );
                }
            }
            // Si las estadísticas tienen un rango de crecimiento
            else if (mItem.TryGetRangeFor(stat, out var range))
            {
                var statGiven = mItem.StatsGiven[statIndex];
                var percentageStatGiven = percentageGivenForStat;
                var statLow = statGiven + range.LowRange;
                var statHigh = statGiven + range.HighRange;

                var statMessage = Strings.ItemDescription.StatGrowthRange.ToString(statLow, statHigh);

                if (percentageStatGiven != 0)
                {
                    statMessage = Strings.ItemDescription.RegularAndPercentage.ToString(
                        statMessage,
                        percentageStatGiven
                    );
                }

                // Mostrar el rango y las diferencias
                rows.AddKeyValueRow(statLabel, statMessage, CustomColors.ItemDesc.Muted, Color.White);
                DisplayKeyValueRowWithDifferenceAndPercent(
                    statDiff,
                    percentageGivenForStat,
                    statLabel,
                    statGiven.ToString(),
                    rows
                );
            }
        }



        // Bonus Effects
        foreach (var effect in mItem.Effects)
        {
            if (effect.Type != ItemEffect.None && effect.Percentage != 0)
            {
                var bonusDiff = GetBonusEffectDifference(effect.Type);
                DisplayKeyValueRowWithDifference(bonusDiff, Strings.ItemDescription.BonusEffects[(int)effect.Type], Strings.ItemDescription.Percentage.ToString(effect.Percentage), rows);
            }
        }

        // Resize the container.
        rows.SizeToChildren(true, true);
    }

    private int GetAttackSpeedDifference()
    {
        var equippedSpeed = GetEquippedWeapon()?.AttackSpeedValue ?? 0;
        return mItem.AttackSpeedValue - equippedSpeed;
    }

    protected void SetupConsumableInfo()
    {
        // Add a divider.
        AddDivider();

        // Add a row component.
        var rows = AddRowContainer();

        // Consumable data.
        if (mItem.Consumable != null)
        {
            if (mItem.Consumable.Value > 0 && mItem.Consumable.Percentage > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], Strings.ItemDescription.RegularAndPercentage.ToString(mItem.Consumable.Value, mItem.Consumable.Percentage));
            }
            else if (mItem.Consumable.Value > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], mItem.Consumable.Value.ToString());
            }
            else if (mItem.Consumable.Percentage > 0)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], Strings.ItemDescription.Percentage.ToString(mItem.Consumable.Percentage));
            }
        }

        // Resize and position the container.
        rows.SizeToChildren(true, true);
    }

    protected void SetupSpellInfo()
    {
        // Add a divider.
        AddDivider();

        // Add a row component.
        var rows = AddRowContainer();

        // Spell data.
        if (mItem.Spell != null)
        {
            if (mItem.QuickCast)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.CastSpell.ToString(mItem.Spell.Name), string.Empty);
            }
            else
            {
                rows.AddKeyValueRow(Strings.ItemDescription.TeachSpell.ToString(mItem.Spell.Name), string.Empty);
            }

            if (mItem.SingleUse)
            {
                rows.AddKeyValueRow(Strings.ItemDescription.SingleUse, string.Empty);
            }
        }

        // Resize and position the container.
        rows.SizeToChildren(true, true);
    }

    protected void SetupBagInfo()
    {
        // Add a divider.
        AddDivider();

        // Add a row component.
        var rows = AddRowContainer();

        // Bag data.
        rows.AddKeyValueRow(Strings.ItemDescription.BagSlots, mItem.SlotCount.ToString());

        // Resize and position the container.
        rows.SizeToChildren(true, true);
    }

    protected void SetupExtraInfo()
    {
        // Our list of data to add, should we need to.
        var data = new List<Tuple<string, string>>();

        // Display our amount, but only if we are stackable and have more than one.
        if (mItem.IsStackable && mAmount > 1)
        {
            data.Add(new Tuple<string, string>(Strings.ItemDescription.Amount, mAmount.ToString("N0").Replace(",", Strings.Numbers.Comma)));
        }

        // Display item drop chance if configured.
        if (mItem.DropChanceOnDeath > 0)
        {
            data.Add(new Tuple<string, string>(Strings.ItemDescription.DropOnDeath, Strings.ItemDescription.Percentage.ToString(mItem.DropChanceOnDeath)));
        }

        // Display shop value if we have one.
        if (!string.IsNullOrWhiteSpace(mValueLabel))
        {
            data.Add(new Tuple<string, string>(mValueLabel, string.Empty));
        }

        // Do we have any data to display? If so, generate the element and add the data to it.
        if (data.Count > 0)
        {
            // Add a divider.
            AddDivider();

            // Add a row component.
            var rows = AddRowContainer();

            foreach (var item in data)
            {
                rows.AddKeyValueRow(item.Item1, item.Item2);
            }

            // Resize and position the container.
            rows.SizeToChildren(true, true);
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        mSpellDescWindow?.Dispose();
    }

    private void DisplayKeyValueRowWithDifference(int statDiff, string keyString, string valueString, Components.RowContainerComponent rows, string unit = "")
    {
        // Si hay una diferencia, mostrar con el color adecuado
        if (statDiff != 0)
        {
            if (Math.Sign(statDiff) > 0)
            {
                // Diferencia positiva
                rows.AddKeyValueRow(
                    keyString,
                    $"{valueString} (+{statDiff}{unit})",
                    CustomColors.ItemDesc.Muted,
                    CustomColors.ItemDesc.Better
                );
            }
            else
            {
                // Diferencia negativa
                rows.AddKeyValueRow(
                    keyString,
                    $"{valueString} ({statDiff}{unit})",
                    CustomColors.ItemDesc.Muted,
                    CustomColors.ItemDesc.Worse
                );
            }
        }
        else
        {
            // Sin diferencia: solo mostrar el valor base
            rows.AddKeyValueRow(
                keyString,
                valueString,
                CustomColors.ItemDesc.Muted, CustomColors.ItemDesc.Muted
            );
        }
    }


    private void DisplayKeyValueRowWithDifferenceAndPercent(int statDiff, int percentDiff, string keyString, string valueString, Components.RowContainerComponent rows)
    {
        string statString = statDiff != 0
            ? (Math.Sign(statDiff) > 0 ? $"+{statDiff}" : $"{statDiff}")
            : "";

        string percentString = percentDiff != 0
            ? (Math.Sign(percentDiff) > 0 ? $"+{percentDiff}%" : $"{percentDiff}%")
            : "";

        var color = Math.Sign(statDiff) > 0
            ? CustomColors.ItemDesc.Better
            : CustomColors.ItemDesc.Worse;

        if (statDiff != 0 || percentDiff != 0)
        {
            rows.AddKeyValueRow(keyString, $"{valueString} ({statString}, {percentString})", CustomColors.ItemDesc.Muted, color);
        }
        else
        {
            rows.AddKeyValueRow(keyString, valueString, CustomColors.ItemDesc.Muted,color);
        }
    }

    private void DisplayKeyValueRowWithDifference(
        double statDiff,
        string keyString,
        string valueString,
        Components.RowContainerComponent rows,
        string unit = "")
    {
        if (statDiff != 0)
        {
            var differenceText = Math.Sign(statDiff) > 0
                ? $"+{statDiff.ToString("0.##")}{unit}"
                : $"{statDiff.ToString("0.##")}{unit}";

            rows.AddKeyValueRow(keyString, $"{valueString} ({differenceText})");
        }
        else
        {
            rows.AddKeyValueRow(keyString, valueString);
        }
    }

}
