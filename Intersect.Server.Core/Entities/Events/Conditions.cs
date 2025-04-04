using Intersect.Enums;
using Intersect.Framework.Core;
using Intersect.Framework.Core.GameObjects.Conditions;
using Intersect.Framework.Core.GameObjects.Conditions.ConditionMetadata;
using Intersect.Framework.Core.GameObjects.Events;
using Intersect.Framework.Core.GameObjects.Variables;
using Intersect.GameObjects;
using Intersect.Server.General;
using Intersect.Server.Maps;

namespace Intersect.Server.Entities.Events;

public static partial class Conditions
{
    public static bool CanSpawnPage(EventPage page, Player player, Event activeInstance)
    {
        return MeetsConditionLists(page.ConditionLists, player, activeInstance);
    }

    public static bool MeetsConditionLists(
        ConditionLists lists,
        Player player,
        Event eventInstance,
        bool singleList = true,
        QuestDescriptor questDescriptor = null
    )
    {
        if (player == null)
        {
            return false;
        }

        //If no condition lists then this passes
        if (lists.Lists.Count == 0)
        {
            return true;
        }

        for (var i = 0; i < lists.Lists.Count; i++)
        {
            if (MeetsConditionList(lists.Lists[i], player, eventInstance, questDescriptor))

            //Checks to see if all conditions in this list are met
            {
                //If all conditions are met.. and we only need a single list to pass then return true
                if (singleList)
                {
                    return true;
                }

                continue;
            }

            //If not.. and we need all lists to pass then return false
            if (!singleList)
            {
                return false;
            }
        }

        //There were condition lists. If single list was true then we failed every single list and should return false.
        //If single list was false (meaning we needed to pass all lists) then we've made it.. return true.
        return !singleList;
    }

    public static bool MeetsConditionList(
        ConditionList list,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        for (var i = 0; i < list.Conditions.Count; i++)
        {
            var meetsCondition = MeetsCondition(list.Conditions[i], player, eventInstance, questDescriptor);

            if (!meetsCondition)
            {
                return false;
            }
        }

        return true;
    }

    

    public static bool MeetsCondition(
        Condition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var result = ConditionHandlerRegistry.CheckCondition(condition, player, eventInstance, questDescriptor);
        if (condition.Negated)
        {
            result = !result;
        }
        return result;
    }

    public static bool MeetsCondition(
        VariableIsCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        VariableValue value = null;
        if (condition.VariableType == VariableType.PlayerVariable)
        {
            value = player.GetVariableValue(condition.VariableId);
        }
        else if (condition.VariableType == VariableType.ServerVariable)
        {
            value = ServerVariableDescriptor.Get(condition.VariableId)?.Value;
        }
        else if (condition.VariableType == VariableType.GuildVariable)
        {
            value = player.Guild?.GetVariableValue(condition.VariableId);
        }
        else if (condition.VariableType == VariableType.UserVariable)
        {
            value = player.User.GetVariableValue(condition.VariableId);
        }

        if (value == null)
        {
            value = new VariableValue();
        }

        return CheckVariableComparison(value, condition.Comparison, player, eventInstance);
    }

    public static bool MeetsCondition(
        HasItemCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var quantity = condition.Quantity;
        if (condition.UseVariable)
        {
            switch (condition.VariableType)
            {
                case VariableType.PlayerVariable:
                    quantity = (int)player.GetVariableValue(condition.VariableId).Integer;

                    break;
                case VariableType.ServerVariable:
                    quantity = (int)ServerVariableDescriptor.Get(condition.VariableId)?.Value.Integer;

                    break;
                case VariableType.GuildVariable:
                    quantity = (int)player.Guild?.GetVariableValue(condition.VariableId).Integer;

                    break;
            }
        }

        return player.CountItems(condition.ItemId, true, condition.CheckBank) >= quantity;
    }

    public static bool MeetsCondition(
        ClassIsCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (player.ClassId == condition.ClassId)
        {
            return true;
        }

        return false;
    }

    public static bool MeetsCondition(
        KnowsSpellCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (player.KnowsSpell(condition.SpellId))
        {
            return true;
        }

        return false;
    }

    public static bool MeetsCondition(
        LevelOrStatCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var lvlStat = 0;
        if (condition.ComparingLevel)
        {
            lvlStat = player.Level;
        }
        else
        {
            lvlStat = player.Stat[(int)condition.Stat].Value();
            if (condition.IgnoreBuffs)
            {
                lvlStat = player.Stat[(int)condition.Stat].BaseStat +
                          player.StatPointAllocations[(int)condition.Stat];
            }
        }

        switch (condition.Comparator) //Comparator
        {
            case VariableComparator.Equal:
                if (lvlStat == condition.Value)
                {
                    return true;
                }

                break;
            case VariableComparator.GreaterOrEqual:
                if (lvlStat >= condition.Value)
                {
                    return true;
                }

                break;
            case VariableComparator.LesserOrEqual:
                if (lvlStat <= condition.Value)
                {
                    return true;
                }

                break;
            case VariableComparator.Greater:
                if (lvlStat > condition.Value)
                {
                    return true;
                }

                break;
            case VariableComparator.Less:
                if (lvlStat < condition.Value)
                {
                    return true;
                }

                break;
            case VariableComparator.NotEqual:
                if (lvlStat != condition.Value)
                {
                    return true;
                }

                break;
        }

        return false;
    }

    public static bool MeetsCondition(
        SelfSwitchCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (eventInstance != null)
        {
            if (eventInstance.Global && MapController.TryGetInstanceFromMap(eventInstance.MapId, player.MapInstanceId, out var instance))
            {
                if (instance.GlobalEventInstances.TryGetValue(eventInstance.Descriptor, out Event evt))
                {
                    if (evt != null)
                    {
                        return evt.SelfSwitch[condition.SwitchIndex] == condition.Value;
                    }
                }
            }
            else
            {
                return eventInstance.SelfSwitch[condition.SwitchIndex] == condition.Value;
            }
        }

        return false;
    }

    public static bool MeetsCondition(
        AccessIsCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var power = player.Power;
        if (condition.Access == 0)
        {
            return power.Ban || power.Kick || power.Mute;
        }
        else if (condition.Access > 0)
        {
            return power.Editor;
        }

        return false;
    }

    public static bool MeetsCondition(
        TimeBetweenCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (condition.Ranges[0] > -1 &&
            condition.Ranges[1] > -1 &&
            condition.Ranges[0] < 1440 / DaylightCycleDescriptor.Instance.RangeInterval &&
            condition.Ranges[1] < 1440 / DaylightCycleDescriptor.Instance.RangeInterval)
        {
            return Time.GetTimeRange() >= condition.Ranges[0] && Time.GetTimeRange() <= condition.Ranges[1];
        }

        return true;
    }

    public static bool MeetsCondition(
        CanStartQuestCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var startQuest = QuestDescriptor.Get(condition.QuestId);
        if (startQuest == questDescriptor)
        {
            //We cannot check and see if we meet quest requirements if we are already checking to see if we meet quest requirements :P
            return true;
        }

        if (startQuest != null)
        {
            return player.CanStartQuest(startQuest);
        }

        return false;
    }

    public static bool MeetsCondition(
        QuestInProgressCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        return player.QuestInProgress(condition.QuestId, condition.Progress, condition.TaskId);
    }

    public static bool MeetsCondition(
        QuestCompletedCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        return player.QuestCompleted(condition.QuestId);
    }

    public static bool MeetsCondition(
        NoNpcsOnMapCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        var map = MapController.Get(eventInstance?.MapId ?? Guid.Empty);
        if (map == null)
        {
            // If we couldn't get an entity's map, use the player's map
            map = MapController.Get(player.MapId);
        }

        if (map != null && map.TryGetInstance(player.MapInstanceId, out var mapInstance))
        {
            var entities = mapInstance.GetEntities();
            foreach (var en in entities)
            {
                if (en is Npc npc)
                {
                    if (!condition.SpecificNpc || npc.Descriptor?.Id == condition.NpcId)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    public static bool MeetsCondition(
        GenderIsCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        return player.Gender == condition.Gender;
    }

    public static bool MeetsCondition(
        MapIsCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        return player.MapId == condition.MapId;
    }

    public static bool MeetsCondition(
        IsItemEquippedCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (player == null || condition == null)
        {
            return false;
        }

        var equipmentIds = player.EquippedItems.Select(item => item.Descriptor.Id).ToArray();

        return equipmentIds?.Contains(condition.ItemId) ?? false;
    }

    public static bool MeetsCondition(
        CheckEquippedSlot condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        if (player == null || condition == null)
        {
            return false;
        }

        var equipmentIndex = Options.Instance.Equipment.Slots.IndexOf(condition.Name);
        return player.TryGetEquipmentSlot(equipmentIndex, out _);
    }

    public static bool MeetsCondition(
        HasFreeInventorySlots condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {

        var quantity = condition.Quantity;
        if (condition.UseVariable)
        {
            switch (condition.VariableType)
            {
                case VariableType.PlayerVariable:
                    quantity = (int)player.GetVariableValue(condition.VariableId).Integer;

                    break;
                case VariableType.ServerVariable:
                    quantity = (int)ServerVariableDescriptor.Get(condition.VariableId)?.Value.Integer;

                    break;
                case VariableType.GuildVariable:
                    quantity = (int)player.Guild?.GetVariableValue(condition.VariableId).Integer;

                    break;
            }
        }

        // Check if the user has (or does not have when negated) the desired amount of inventory slots.
        var slots = player.FindOpenInventorySlots().Count;

        return slots >= quantity;
    }

    public static bool MeetsCondition(
        InGuildWithRank condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor
    )
    {
        return player.Guild != null && player.GuildRank <= condition.Rank;
    }

    public static bool MeetsCondition(
        MapZoneTypeIs condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor)
    {
        return player.Map?.ZoneType == condition.ZoneType;
    }

    public static bool MeetsCondition(
        CombatCondition condition,
        Player player,
        Event eventInstance,
        QuestDescriptor questDescriptor)
    {
        return player.CombatTimer > Timing.Global.Milliseconds;
    }

    //Variable Comparison Processing

    public static bool CheckVariableComparison(
        VariableValue currentValue,
        VariableComparison comparison,
        Player player,
        Event instance
    )
    {
        return VariableCheckHandlerRegistry.CheckVariableComparison(currentValue, comparison, player, instance);
    }

    public static bool CheckVariableComparison(
        VariableValue currentValue,
        BooleanVariableComparison comparison,
        Player player,
        Event instance
    )
    {
        VariableValue compValue = null;
        if (comparison.CompareVariableId != Guid.Empty)
        {
            if (comparison.CompareVariableType == VariableType.PlayerVariable)
            {
                compValue = player.GetVariableValue(comparison.CompareVariableId);
            }
            else if (comparison.CompareVariableType == VariableType.ServerVariable)
            {
                compValue = ServerVariableDescriptor.Get(comparison.CompareVariableId)?.Value;
            }
            else if (comparison.CompareVariableType == VariableType.GuildVariable)
            {
                compValue = player.Guild?.GetVariableValue(comparison.CompareVariableId);
            }
            else if (comparison.CompareVariableType == VariableType.UserVariable)
            {
                compValue = player.User.GetVariableValue(comparison.CompareVariableId);
            }
        }
        else
        {
            compValue = new VariableValue();
            compValue.Boolean = comparison.Value;
        }

        if (compValue == null)
        {
            compValue = new VariableValue();
        }

        if (currentValue.Type == 0)
        {
            currentValue.Boolean = false;
        }

        if (compValue.Type != currentValue.Type)
        {
            return false;
        }

        if (comparison.ComparingEqual)
        {
            return currentValue.Boolean == compValue.Boolean;
        }
        else
        {
            return currentValue.Boolean != compValue.Boolean;
        }
    }

    public static bool CheckVariableComparison(
        VariableValue currentValue,
        IntegerVariableComparison comparison,
        Player player,
        Event instance
    )
    {
        long compareAgainst = 0;

        VariableValue compValue = null;
        if (comparison.CompareVariableId != Guid.Empty)
        {
            if (comparison.CompareVariableType == VariableType.PlayerVariable)
            {
                compValue = player.GetVariableValue(comparison.CompareVariableId);
            }
            else if (comparison.CompareVariableType == VariableType.ServerVariable)
            {
                compValue = ServerVariableDescriptor.Get(comparison.CompareVariableId)?.Value;
            }
            else if (comparison.CompareVariableType == VariableType.GuildVariable)
            {
                compValue = player.Guild?.GetVariableValue(comparison.CompareVariableId);
            }
            else if (comparison.CompareVariableType == VariableType.UserVariable)
            {
                compValue = player.User.GetVariableValue(comparison.CompareVariableId);
            }
        }
        else if (comparison.TimeSystem)
        {
            compValue = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
        else
        {
            compValue = new VariableValue();
            compValue.Integer = comparison.Value;
        }

        if (compValue == null)
        {
            compValue = new VariableValue();
        }

        if (currentValue.Type == 0)
        {
            currentValue.Integer = 0;
        }

        if (compValue.Type != currentValue.Type)
        {
            return false;
        }

        var varVal = currentValue.Integer;
        compareAgainst = compValue.Integer;

        switch (comparison.Comparator) //Comparator
        {
            case VariableComparator.Equal:
                if (varVal == compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.GreaterOrEqual:
                if (varVal >= compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.LesserOrEqual:
                if (varVal <= compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.Greater:
                if (varVal > compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.Less:
                if (varVal < compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.NotEqual:
                if (varVal != compareAgainst)
                {
                    return true;
                }

                break;
            case VariableComparator.Between:
                if (varVal >= comparison.Value && varVal <= comparison.MaxValue)
                {
                    return true;
                }

                break;
        }

        return false;
    }

    public static bool CheckVariableComparison(
        VariableValue currentValue,
        StringVariableComparison comparison,
        Player player,
        Event instance
    )
    {
        var varVal = CommandProcessing.ParseEventText(currentValue.String ?? "", player, instance);
        var compareAgainst = CommandProcessing.ParseEventText(comparison.Value ?? "", player, instance);

        switch (comparison.Comparator)
        {
            case StringVariableComparator.Equal:
                return varVal == compareAgainst;
            case StringVariableComparator.Contains:
                return varVal.Contains(compareAgainst);
        }

        return false;
    }

}
