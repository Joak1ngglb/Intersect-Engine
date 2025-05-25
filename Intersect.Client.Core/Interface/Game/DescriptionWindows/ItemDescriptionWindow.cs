
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.GameObjects.Ranges;
using Intersect.Logging;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Interface.Game.DescriptionWindows.Components;
using Intersect.Client.Framework.Gwen;

namespace Intersect.Client.Interface.Game.DescriptionWindows;

public partial class ItemDescriptionWindow : DescriptionWindowBase
{
    protected ItemBase mItem;
    protected int mAmount;
    protected ItemProperties? mItemProperties;
    protected string mTitleOverride;
    protected string mValueLabel;
    protected SpellDescriptionWindow? mSpellDescWindow;

    public ItemDescriptionWindow(ItemBase item, int amount, int x, int y, ItemProperties? itemProperties, string titleOverride = "", string valueLabel = "")
        : base(Interface.GameUi.GameCanvas, "DescriptionWindow")
    {
        mItem = item;
        mAmount = amount;
        mItemProperties = itemProperties;
        mTitleOverride = titleOverride;
        mValueLabel = valueLabel;

        GenerateComponents();
        SetupDescriptionWindow();
        SetPosition(x, y);
   
        if (mItem.ItemType == ItemType.Spell)
        {
            mSpellDescWindow = new SpellDescriptionWindow(mItem.SpellId, x, y, mContainer);
        }
    }

    protected void SetupDescriptionWindow()
    {
        if (mItem == null) return;

        SetupHeader();
        SetupItemLimits();
        if (!string.IsNullOrWhiteSpace(mItem.Description)) SetupDescription();

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
            case ItemType.Resource:
                SetupResourceInfo();
                break;
        }

        SetupExtraInfo();
        FinalizeWindow();
    }

    protected void SetupHeader()
    {
        var header = AddHeader();
        var tex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, mItem.Icon);
        if (tex != null) header.SetIcon(tex, mItem.Color);

        var rarityColor = CustomColors.Items.Rarities.TryGetValue(mItem.Rarity, out var rColor) ? rColor : Color.White;
        header.SetBackgroundColor(rarityColor);

        var name = !string.IsNullOrWhiteSpace(mTitleOverride) ? mTitleOverride : mItem.Name;
        var itemLevel = mItemProperties?.EnchantmentLevel ?? 0;
        if (itemLevel > 0) name += $" +{itemLevel}";

        header.SetTitle(name, rarityColor);

        Strings.ItemDescription.ItemTypes.TryGetValue((int)mItem.ItemType, out var typeDesc);
        var subtypeText = !string.IsNullOrWhiteSpace(mItem.Subtype) ? $"{mItem.Subtype}" : "";
        if (mItem.ItemType == ItemType.Equipment)
        {
            var equipSlot = Options.Equipment.Slots[mItem.EquipmentSlot];

            // 🔥 Si es arma y tiene subtipo, mostrar solo el subtipo
            if (mItem.EquipmentSlot == Options.WeaponIndex && !string.IsNullOrWhiteSpace(mItem.Subtype))
            {
                header.SetSubtitle($"{mItem.Subtype}", Color.White);
            }
            else
            {
                // 🔥 Mostrar info extra (como TwoHanded) si no tiene subtipo o no es arma
                var extraInfo = mItem.EquipmentSlot == Options.WeaponIndex && mItem.TwoHanded
                    ? $"{Strings.ItemDescription.TwoHand} {equipSlot}"
                    : equipSlot;

                header.SetSubtitle($"{extraInfo}", Color.White);
            }
        }
        else
        {
            // 🔥 Para ítems que NO son equipo: mostrar tipo + subtipo si hay.
            var subtypeInfo = !string.IsNullOrWhiteSpace(mItem.Subtype) ? $"{mItem.Subtype}" : "";
            header.SetSubtitle($"{subtypeInfo}", Color.White);
        }


        try
        {
            if (Options.Instance.Items.TryGetRarityName(mItem.Rarity, out var rarityName))
            {
                _ = Strings.ItemDescription.Rarity.TryGetValue(rarityName, out var rarityLabel);
                header.SetDescription(rarityLabel, rarityColor);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }

        header.SizeToChildren(true, false);
    }

    protected void SetupItemLimits()
    {
        var limits = new List<string>();
        if (!mItem.CanBank) limits.Add(Strings.ItemDescription.Banked);
        if (!mItem.CanGuildBank) limits.Add(Strings.ItemDescription.GuildBanked);
        if (!mItem.CanBag) limits.Add(Strings.ItemDescription.Bagged);
        if (!mItem.CanTrade) limits.Add(Strings.ItemDescription.Traded);
        if (!mItem.CanDrop) limits.Add(Strings.ItemDescription.Dropped);
        if (!mItem.CanSell) limits.Add(Strings.ItemDescription.Sold);

        if (limits.Count > 0)
        {
            AddDivider();
            var description = AddDescription();
            description.AddText(Strings.ItemDescription.ItemLimits.ToString(string.Join(", ", limits)), Color.White);
        }
    }

    protected void SetupDescription()
    {
        AddDivider();
        var description = AddDescription();
        description.AddText(Strings.ItemDescription.Description.ToString(mItem.Description), Color.White);
    }

    protected void SetupEquipmentInfo()
    {
        AddDivider();
        var rows = AddRowContainer();
       
        if (mItem.EquipmentSlot == Options.WeaponIndex)
        {
            DisplayKeyValueRowWithDifference(GetBaseDamageDifference(), Strings.ItemDescription.BaseDamage, mItem.Damage.ToString(), rows);

            Strings.ItemDescription.DamageTypes.TryGetValue(mItem.DamageType, out var damageType);
            rows.AddKeyValueRow(Strings.ItemDescription.BaseDamageType, damageType);

            if (mItem.Scaling > 0)
            {
                Strings.ItemDescription.Stats.TryGetValue(mItem.ScalingStat, out var stat);
                DisplayKeyValueRowWithDifference(GetScalingDifference(), Strings.ItemDescription.ScalingStat, stat, rows);
                rows.AddKeyValueRow(Strings.ItemDescription.ScalingPercentage, Strings.ItemDescription.Percentage.ToString(mItem.Scaling));
            }

            if (mItem.CritChance > 0)
            {
                DisplayKeyValueRowWithDifference(GetCritChanceDifference(), Strings.ItemDescription.CritChance, Strings.ItemDescription.Percentage.ToString(mItem.CritChance), rows);
                DisplayKeyValueRowWithDifference(GetCritMultiplierDifference(), Strings.ItemDescription.CritMultiplier, Strings.ItemDescription.Multiplier.ToString(mItem.CritMultiplier), rows);
            }
            // 🔥 NUEVO BLOQUE CORREGIDO
            if (mItem.AttackSpeedModifier == 0)
            {
                var baseSpeed = Globals.Me.Stat[(int)Stat.Speed];

                // Stats del arma equipada (si existe)
                var weaponSlot = Globals.Me.MyEquipment[Options.WeaponIndex];
                if (weaponSlot != -1)
                {
                    var equippedWeapon = Globals.Me.Inventory[weaponSlot];
                    var baseWeapon = equippedWeapon.Base;
                    var randomStats = equippedWeapon.ItemProperties.StatModifiers;

                    baseSpeed = (int)Math.Round(baseSpeed / ((100 + baseWeapon.PercentageStatsGiven[(int)Stat.Speed]) / 100f));
                    baseSpeed -= baseWeapon.StatsGiven[(int)Stat.Speed];
                    baseSpeed -= randomStats[(int)Stat.Speed];
                }

                // Ajustes del ítem actual
                if (mItemProperties?.StatModifiers != null)
                {
                    baseSpeed += mItem.StatsGiven[(int)Stat.Speed];
                    baseSpeed += mItemProperties.StatModifiers[(int)Stat.Speed];
                    baseSpeed += (int)Math.Floor(baseSpeed * (mItem.PercentageStatsGiven[(int)Stat.Speed] / 100f));
                }

                var attackTime = Globals.Me.CalculateAttackTime(baseSpeed);

                // Comparar con el arma equipada
                var equippedAttackTime = GetEquippedWeapon() != null ? GetEquippedWeapon().AttackSpeedValue : 0;
                var attackSpeedDiff = attackTime - equippedAttackTime;

                DisplayKeyValueRowWithDifference(attackSpeedDiff, Strings.ItemDescription.AttackSpeed, TimeSpan.FromMilliseconds(attackTime).WithSuffix(), rows);
            }
            else if (mItem.AttackSpeedModifier == 1)
            {
                DisplayKeyValueRowWithDifference(GetAttackSpeedDifference(), Strings.ItemDescription.AttackSpeed, TimeSpan.FromMilliseconds(mItem.AttackSpeedValue).WithSuffix(), rows);
            }
            else if (mItem.AttackSpeedModifier == 2)
            {
                DisplayKeyValueRowWithDifference(GetAttackSpeedDifference(), Strings.ItemDescription.AttackSpeed, Strings.ItemDescription.Percentage.ToString(mItem.AttackSpeedValue), rows);
            }

        }

        if (mItem.EquipmentSlot == Options.ShieldIndex)
        {
            if (mItem.BlockChance > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.BlockChance, Strings.ItemDescription.Percentage.ToString(mItem.BlockChance));
            if (mItem.BlockAmount > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.BlockAmount, Strings.ItemDescription.Percentage.ToString(mItem.BlockAmount));
            if (mItem.BlockAbsorption > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.BlockAbsorption, Strings.ItemDescription.Percentage.ToString(mItem.BlockAbsorption));
        }

        SetupVitalsAndStats(rows);
        SetupBonusEffects(rows);
        // Mostrar conteo de orbes aplicados por stat (uno por fila)
        if (mItemProperties?.StatOrbUpgradeCounts != null)
        {
            bool hasOrbs = false;
           
            for (int i = 0; i < mItemProperties.StatOrbUpgradeCounts.Length; i++)
            {
                int orbCount = mItemProperties.StatOrbUpgradeCounts[i];
                if (orbCount > 0)
                {
                    if (!hasOrbs)
                    {
                        AddDivider(); // 🔥 Divisor visual antes de mostrar los orbes
                        hasOrbs = true;
                    }

                    Strings.ItemDescription.Stats.TryGetValue(i, out var statName);
                    rows.AddKeyValueRow($"{statName} (Orbes)", $"+{orbCount}", CustomColors.ItemDesc.Muted, Color.Orange);
                }
            }
        }
      
        rows.SizeToChildren(true, true);
    }
    protected void SetupSetsComponets()
    {
        if (mItem.ItemType != ItemType.Equipment || mItem.SetId == Guid.Empty)
            return;

        var set = SetBase.Get(mItem.SetId);
        if (set == null || set.ItemIds.Count == 0)
            return;

        AddDivider();
        var descComponent = AddDescription();
        descComponent.AddText(Strings.ItemDescription.SetName.ToString(set.Name), Color.Yellow);
    
        // Bloque de bonus: usa filas
            var BonusDesc = AddDescription();
        var bonusRows = new RowContainerComponent(BonusDesc.Container, "SetBonusRows");

       
        // Conteo de equipados
        var equippedCount = set.ItemIds.Count(id => PlayerInfo.HasItemEquipped(id));
        var ratio = equippedCount / (float)set.ItemIds.Count;

        // Muestra bonus escalados solo si hay al menos 1 equipado
        if (equippedCount > 1)
        {
            bonusRows.AddKeyValueRow("Set Bonus", null, Color.White, null);
            var (stats, percentStats, vitals, percentVitals, effects) = set.GetBonuses(ratio);

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i] != 0 || percentStats[i] != 0)
                {
                    var label = Strings.ItemDescription.StatCounts[i];
                    var value = $"{stats[i]}";
                    if (percentStats[i] != 0) value += $" / {percentStats[i]}%";
                    bonusRows.AddKeyValueRow(label, value, Color.Magenta, Color.White);
                }
            }

            for (int i = 0; i < vitals.Length; i++)
            {
                if (vitals[i] != 0 || percentVitals[i] != 0)
                {
                    var label = Strings.ItemDescription.Vitals[i];
                    var value = $"{vitals[i]}";
                    if (percentVitals[i] != 0) value += $" / {percentVitals[i]}%";
                    bonusRows.AddKeyValueRow(label, value, Color.Cyan, Color.White);
                }
            }

            foreach (var effect in effects)
            {
                if (effect.Percentage > 0)
                {
                    var label = Strings.ItemDescription.BonusEffects[(int)effect.Type];
                    bonusRows.AddKeyValueRow(label, $"+{effect.Percentage}%", Color.Yellow, Color.White);
                }
            }
        }

        bonusRows.SizeToChildren(true, true);
        descComponent.SizeToChildren(true, true);
        AddDivider();
        var setContainer = AddDescription();


        // 🔷 Contenedor horizontal para íconos
        var iconRow = new RowItemContainerComponent(setContainer.Container, "SetIconRow");

        foreach (var itemId in set.ItemIds)
        {
            var memberItem = ItemBase.Get(itemId);
            if (memberItem == null) continue;

            var setItem = new SetItemComponent(iconRow.Container, "SetItem");
            var tex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, memberItem.Icon);
            setItem.AddItem(memberItem, PlayerInfo.HasItemEquipped(itemId));
            iconRow.AddItemComponent(setItem);
        }

        // 🔁 Ajustes visuales
        iconRow.SizeToChildren(true, false);
        setContainer.SizeToChildren(true, true);
    }


    protected void SetupVitalsAndStats(Components.RowContainerComponent rows)
    {
        // Vitals: Vida, Mana, etc.
        for (var i = 0; i < Enum.GetValues<Vital>().Length; i++)
        {
            var vitalLabel = Strings.ItemDescription.Vitals[i];
            var flat = mItem.VitalsGiven[i];
            var percent = mItem.PercentageVitalsGiven[i];
            var regen = mItem.VitalsRegen[i];

            var valueParts = new List<string>();
            if (flat != 0) valueParts.Add($"{flat}");
            if (percent != 0) valueParts.Add($"{percent}%");
            if (regen > 0) valueParts.Add($"Regen {regen}%");

            if (valueParts.Count > 0)
            {
                rows.AddKeyValueRow(vitalLabel, string.Join(" / ", valueParts), CustomColors.ItemDesc.Muted, Color.White);
            }
        }

        // Stats: Fuerza, Magia, etc.
        for (var i = 0; i < Enum.GetValues<Stat>().Length; i++)
        {
            var statLabel = Strings.ItemDescription.StatCounts[i];
            var flat = mItem.StatsGiven[i];
            var percent = mItem.PercentageStatsGiven[i];

            // SUMAR modificadores aleatorios si hay:
            if (mItemProperties?.StatModifiers != null)
            {
                flat += mItemProperties.StatModifiers[i];
            }

            var diff = GetStatDifference(i); // Compara vs equipado

            if (flat != 0 || percent != 0)
            {
                var valueParts = new List<string>();
                if (flat != 0) valueParts.Add(flat.ToString());
                if (percent != 0) valueParts.Add($"{percent}%");

                var valueString = string.Join(" / ", valueParts);
                var diffString = FormatDifference(diff);
                var color = GetDiffColor(diff);

                rows.AddKeyValueRow(statLabel, $"{valueString}{diffString}", CustomColors.ItemDesc.Muted, color);
            }
        }
    }

    protected void SetupBonusEffects(Components.RowContainerComponent rows)
    {
        foreach (var effect in mItem.Effects.Where(e => e.Type != ItemEffect.None && e.Percentage != 0))
        {
            var label = Strings.ItemDescription.BonusEffects[(int)effect.Type];
            var value = $"{effect.Percentage}%";
            var diff = GetBonusEffectDifference(effect.Type);
            var color = GetDiffColor(diff);
            rows.AddKeyValueRow(label, value, CustomColors.ItemDesc.Muted, color);
        }
    }

    protected void SetupConsumableInfo()
    {
        AddDivider();
        var rows = AddRowContainer();
        if (mItem.Consumable != null)
        {
            if (mItem.Consumable.Value > 0 && mItem.Consumable.Percentage > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], Strings.ItemDescription.RegularAndPercentage.ToString(mItem.Consumable.Value, mItem.Consumable.Percentage));
            else if (mItem.Consumable.Value > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], mItem.Consumable.Value.ToString());
            else if (mItem.Consumable.Percentage > 0)
                rows.AddKeyValueRow(Strings.ItemDescription.ConsumableTypes[(int)mItem.Consumable.Type], Strings.ItemDescription.Percentage.ToString(mItem.Consumable.Percentage));
        }
        rows.SizeToChildren(true, true);
    }

    protected void SetupSpellInfo()
    {
        AddDivider();
        var rows = AddRowContainer();
        if (mItem.Spell != null)
        {
            var spellInfo = mItem.QuickCast
                ? Strings.ItemDescription.CastSpell.ToString(mItem.Spell.Name)
                : Strings.ItemDescription.TeachSpell.ToString(mItem.Spell.Name);
            rows.AddKeyValueRow(spellInfo, string.Empty);
            if (mItem.SingleUse)
                rows.AddKeyValueRow(Strings.ItemDescription.SingleUse, string.Empty);
        }
        rows.SizeToChildren(true, true);
    }

    protected void SetupBagInfo()
    {
        AddDivider();
        var rows = AddRowContainer();
        rows.AddKeyValueRow(Strings.ItemDescription.BagSlots, mItem.SlotCount.ToString());
        rows.SizeToChildren(true, true);
    }

    protected void SetupResourceInfo()
    {
        AddDivider();
        var rows = AddRowContainer();

      
        if (mItem.Subtype == "Rune")
        {
            var successRate = mItem.UpgradeMaterialSuccessRate;
            rows.AddKeyValueRow("Tasa de Éxito", $"{successRate * 100:0.##}%", CustomColors.ItemDesc.Muted, Color.White);
        }

        if (mItem.Subtype == "Orb")
        {
            var successRate = mItem.UpgradeMaterialSuccessRate;
            var statName = mItem.TargetStat.ToString();
            var amount = mItem.AmountModifier;

            rows.AddKeyValueRow("Tasa de Éxito", $"{successRate * 100:0.##}%", CustomColors.ItemDesc.Muted, Color.White);
            rows.AddKeyValueRow("Stat Modificado", statName, CustomColors.ItemDesc.Muted, Color.White);
            rows.AddKeyValueRow("Cantidad", $"+{amount}", CustomColors.ItemDesc.Muted, Color.White);
        }

        rows.SizeToChildren(true, true);
    }

    protected void SetupExtraInfo()
    {
        var data = new List<Tuple<string, string>>();

        if (mItem.IsStackable && mAmount > 1)
            data.Add(new Tuple<string, string>(Strings.ItemDescription.Amount, mAmount.ToString("N0").Replace(",", Strings.Numbers.Comma)));
        if (mItem.DropChanceOnDeath > 0)
            data.Add(new Tuple<string, string>(Strings.ItemDescription.DropOnDeath, Strings.ItemDescription.Percentage.ToString(mItem.DropChanceOnDeath)));
        if (!string.IsNullOrWhiteSpace(mValueLabel))
            data.Add(new Tuple<string, string>(mValueLabel, string.Empty));

        if (data.Count > 0)
        {
            AddDivider();
            var rows = AddRowContainer();
            foreach (var item in data)
                rows.AddKeyValueRow(item.Item1, item.Item2);
            rows.SizeToChildren(true, true);
        }

        // 🔥 Ahora llama acá tu sección de íconos del set
      SetupSetsComponets();
    }

    public override void Dispose()
    {
        base.Dispose();
        mSpellDescWindow?.Dispose();
    }

    // Métodos utilitarios de diferencia
    private string FormatDifference(int diff) => diff != 0 ? $" ({(diff > 0 ? "+" : "")}{diff})" : "";
    private Color GetDiffColor(int diff) => diff > 0 ? CustomColors.ItemDesc.Better : diff < 0 ? CustomColors.ItemDesc.Worse : CustomColors.ItemDesc.Muted;

    private int GetBaseDamageDifference() => mItem.Damage - (GetEquippedWeapon()?.Damage ?? 0);
    private ItemBase? GetEquippedWeapon() => Globals.Me.MyEquipment[Options.WeaponIndex] != -1 ? Globals.Me.Inventory[Globals.Me.MyEquipment[Options.WeaponIndex]].Base : null;
    private int GetScalingDifference() => mItem.Scaling - (GetEquippedWeapon()?.Scaling ?? 0);
    private int GetCritChanceDifference() => mItem.CritChance - (GetEquippedWeapon()?.CritChance ?? 0);
    private int GetCritMultiplierDifference() => (int)(mItem.CritMultiplier - (GetEquippedWeapon()?.CritMultiplier ?? 0));
    private int GetAttackSpeedDifference() => mItem.AttackSpeedValue - (GetEquippedWeapon()?.AttackSpeedValue ?? 0);
    private int GetBonusEffectDifference(ItemEffect effectType) => (mItem.Effects.FirstOrDefault(e => e.Type == effectType)?.Percentage ?? 0) - (GetEquippedEffect(effectType));
    private int GetEquippedEffect(ItemEffect effectType) => GetEquippedWeapon()?.Effects.FirstOrDefault(e => e.Type == effectType)?.Percentage ?? 0;
    private int GetStatDifference(int statIndex)
    {
        var slot = mItem.EquipmentSlot;
        var newItemStat = mItem.StatsGiven[statIndex] + (mItemProperties?.StatModifiers?[statIndex] ?? 0);
        var equippedItem = Globals.Me.MyEquipment[slot] != -1 ? Globals.Me.Inventory[Globals.Me.MyEquipment[slot]] : null;
        var equippedStat = equippedItem?.Base.StatsGiven[statIndex] + (equippedItem?.ItemProperties?.StatModifiers?[statIndex] ?? 0) ?? 0;
        return newItemStat - equippedStat;
    }

    // Agrega al final de tu clase:

    private void DisplayKeyValueRowWithDifference(int diff, string key, string value, Components.RowContainerComponent rows, string unit = "")
    {
        var diffText = FormatDifference(diff);
        var color = GetDiffColor(diff);
        rows.AddKeyValueRow(key, $"{value}{diffText}", CustomColors.ItemDesc.Muted, color);
    }

    private void DisplayKeyValueRowWithDifferenceAndPercent(int flatDiff, int percentDiff, string key, string value, Components.RowContainerComponent rows)
    {
        var parts = new List<string>();
        if (flatDiff != 0) parts.Add($"{(flatDiff > 0 ? "+" : "")}{flatDiff}");
        if (percentDiff != 0) parts.Add($"{(percentDiff > 0 ? "+" : "")}{percentDiff}%");

        var diffText = parts.Count > 0 ? $" ({string.Join(", ", parts)})" : "";
        var color = flatDiff > 0 ? CustomColors.ItemDesc.Better :
                    flatDiff < 0 ? CustomColors.ItemDesc.Worse :
                    CustomColors.ItemDesc.Muted;

        rows.AddKeyValueRow(key, $"{value}{diffText}", CustomColors.ItemDesc.Muted, color);
    }

    private void DisplayKeyValueRowWithDifference(double diff, string key, string value, Components.RowContainerComponent rows, string unit = "")
    {
        var diffText = diff != 0 ? $" ({(diff > 0 ? "+" : "")}{diff:0.##}{unit})" : "";
        var color = diff > 0 ? CustomColors.ItemDesc.Better :
                    diff < 0 ? CustomColors.ItemDesc.Worse :
                    CustomColors.ItemDesc.Muted;

        rows.AddKeyValueRow(key, $"{value}{diffText}", CustomColors.ItemDesc.Muted, color);
    }

    public static class PlayerInfo
    {
        public static bool HasItemEquipped(Guid itemId)
        {
            return Globals.Me.MyEquipment.Any(index =>
                index > -1 &&
                index < Options.MaxInvItems &&
                Globals.Me.Inventory[index].ItemId == itemId);
        }
    }

}
