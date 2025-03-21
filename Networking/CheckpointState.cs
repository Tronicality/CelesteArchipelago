﻿using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using System.Collections.Generic;

namespace Celeste.Mod.CelesteArchipelago
{
    public class CheckpointState
    {
        private ulong runningTotal;
        public string? LastHitCheckpoint;

        private struct CheckpointItem
        {
            public AreaKey Area;
            public string Level;
            public CheckpointItem(AreaKey area, string level)
            {
                Area = area;
                Level = level;
            }
        }
        private static List<CheckpointItem> Checkpoints;
        private IDataStorageHelper helper;

        private static void InitCheckpoints()
        {
            Checkpoints = new List<CheckpointItem>();
            for (int area = 0; area < AreaData.Areas.Count; area++)
            {
                for (int mode = 0; mode < AreaData.Areas[area].Mode.Length; mode++)
                {
                    if (AreaData.Areas[area].Mode[mode] is not null && AreaData.Areas[area].Mode[mode].Checkpoints is not null)
                    {
                        foreach (var checkpoint in AreaData.Areas[area].Mode[mode].Checkpoints)
                        {
                            Checkpoints.Add(new CheckpointItem(new AreaKey(area, (AreaMode)mode), checkpoint.Level));
                        }
                    }
                }
            }
        }

        private static int FindCheckpoint(AreaKey area, string level)
        {
            return Checkpoints.FindIndex((x) => x.Area == area && x.Level == level);
        }

        public CheckpointState(ulong current, IDataStorageHelper helper)
        {
            runningTotal = current;
            if(Checkpoints is null)
            {
                InitCheckpoints();
            }
            this.helper = helper;
        }

        public void MarkCheckpoint(AreaKey area, string level)
        {
            var idx = FindCheckpoint(area, level);
            if (idx == -1) return;
            runningTotal |= (ulong)1 << idx;
            helper[Scope.Slot, "CelesteCheckpointState"] = unchecked((long)runningTotal + long.MinValue);
        }

        public void UnMarkCheckpoint(AreaKey area, string level)
        {
            var idx = FindCheckpoint(area, level);
            if (idx == -1) return;
            runningTotal &= ~((ulong)1 << idx);
            helper[Scope.Slot, "CelesteCheckpointState"] = unchecked((long)runningTotal + long.MinValue);
        }

        public void ApplyCheckpoints()
        {
            ulong copy = runningTotal;
            int idx = 0;
            while(copy > 0)
            {
                if(copy % 2 == 1)
                {
                    SaveData.Instance.SetCheckpoint(Checkpoints[idx].Area, Checkpoints[idx].Level);
                }
                copy /= 2;
                idx++;
            }
        }

        public void RemoveAreaCheckpoints(AreaKey area)
        {
            // ApSave
            HashSet<string> checkpoints = SaveData.Instance.Areas_Safe[area.ID].Modes[(int)area.Mode].Checkpoints;
            foreach (string level in checkpoints)
            {
                UnMarkCheckpoint(area, level);
            }

            // Local / SaveData
            SaveData.Instance.Areas[area.ID].CleanCheckpoints();
        }

        public void CleanSaveDataCheckpoints()
        {
            for (int area = 0; area < AreaData.Areas.Count; area++)
            {
                for (int mode = 0; mode < AreaData.Areas[area].Mode.Length; mode++)
                {
                    if (SaveData.Instance is not null)
                    {
                        SaveData.Instance.Areas[area].CleanCheckpoints();
                    }
                }
            } 
        }
    }
}