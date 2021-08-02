﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StockpileStackLimit
{
    public static class Limits
    {
        private static readonly Dictionary<StorageSettings, int> limits = new Dictionary<StorageSettings, int>();

        public static int CalculateStackLimit(Thing t)
        {
            if (!t.Spawned)
            {
                return t.def.stackLimit;
            }

            var slotgroup = t.Map.haulDestinationManager.SlotGroupAt(t.Position); //t.GetSlotGroup();
            if (slotgroup == null)
            {
                return t.def.stackLimit;
            }

            var limit = GetLimit(slotgroup.Settings);
            return limit > 0 ? limit : t.def.stackLimit;
        }

        public static int CalculateStackLimit(SlotGroup slotgroup)
        {
            if (slotgroup == null)
            {
                return 99999;
            }

            var setting = slotgroup.Settings;
            var limit = GetLimit(setting);
            return limit > 0 ? limit : 99999;
        }

        public static int CalculateStackLimit(Map map, IntVec3 cell)
        {
            var slotgroup = map.haulDestinationManager.SlotGroupAt(cell);
            if (slotgroup == null)
            {
                return 0;
            }

            var limit = GetLimit(slotgroup.Settings);
            return limit > 0 ? limit : 0;
        }

        public static bool HasStackLimit(Thing t)
        {
            if (!t.Spawned)
            {
                return false;
            }

            var slotgroup = t.Map.haulDestinationManager.SlotGroupAt(t.Position); //t.GetSlotGroup();
            if (slotgroup == null)
            {
                return false;
            }

            var setting = slotgroup.Settings;
            return HasLimit(setting);
        }

        public static bool HasStackLimit(SlotGroup slotgroup)
        {
            if (slotgroup == null)
            {
                return false;
            }

            var setting = slotgroup.Settings;
            return HasLimit(setting);
        }

        public static int GetLimit(StorageSettings settings)
        {
            return !limits.ContainsKey(settings) ? -1 : limits[settings];
        }

        public static void SetLimit(StorageSettings settings, int limit)
        {
            _ = settings.owner as ISlotGroupParent;
            //t.Map.listerHaulables.RecalcAllInCells(t.AllSlotCells());
            if (!limits.ContainsKey(settings))
            {
                limits.Add(settings, limit);
            }
            else
            {
                limits[settings] = limit;
            }
        }

        public static bool HasLimit(StorageSettings settings)
        {
            return limits.ContainsKey(settings) && limits[settings] > 0;
        }
    }
}