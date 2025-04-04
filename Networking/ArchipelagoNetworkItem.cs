﻿using System;
using System.Collections.Generic;

namespace Celeste.Mod.CelesteArchipelago
{
    public class ArchipelagoNetworkItem
    {
        public const int OFFSET_BASE = 8000000;
        public const int OFFSET_KIND = 20000;
        public const int OFFSET_LEVEL = 1000;
        public const int OFFSET_SIDE = 100;

        public CollectableType type;
        public int area;
        public int mode;
        public int offset;
        public EntityID? entity;

        private static Dictionary<int, EntityID> StrawberryMap;
        private static Dictionary<string, int> StrawberryReverseMap;

        public long ID
        {
            get
            {
                return OFFSET_BASE + (int)type * OFFSET_KIND + area * OFFSET_LEVEL + mode * OFFSET_SIDE + offset;
            }
        }

        public AreaKey areaKey
        {
            get
            {
                var areaKey = new AreaKey(0, (AreaMode)mode);
                areaKey.ID = area;
                return areaKey;
            }
        }

        public ArchipelagoNetworkItem(long networkID)
        {
            int temp = (int)(networkID % OFFSET_BASE);

            type = (CollectableType)(temp / OFFSET_KIND);
            temp %= OFFSET_KIND;

            area = temp / OFFSET_LEVEL;
            temp %= OFFSET_LEVEL;

            mode = temp / OFFSET_SIDE;
            temp %= OFFSET_SIDE;

            offset = temp;
            if (type == CollectableType.STRAWBERRY)
            {
                entity = GetStrawberryEntityID(area, mode, offset);
            }
            else if (type == CollectableType.TRAP) {
                entity = GetTrapEntityID((TrapType)offset);
            }
        }

        public ArchipelagoNetworkItem(CollectableType type, int area, int mode, EntityID? entity = null)
        {
            this.type = type;
            this.area = area;
            this.mode = mode;

            if (!entity.HasValue)
            {
                offset = 0;
                this.entity = null;
            }
            else if (type == CollectableType.TRAP)
            {
                this.entity = null;
            }
            else
            {
                offset = (GetStrawberryOffset(entity.Value) ?? 99) % OFFSET_SIDE;
                this.entity = GetStrawberryEntityID(area, mode, offset);
            }
        }

        private static void BuildStrawberryMap()
        {
            StrawberryMap = new Dictionary<int, EntityID>();
            StrawberryReverseMap = new Dictionary<string, int>();

            int offset, id;
            EntityID strawberry;
            foreach (AreaData area in AreaData.Areas)
            {
                for (int i = 0; i < area.Mode.Length; i++)
                {
                    ModeProperties modeProperties = area.Mode[i];
                    offset = 0;
                    var maxJ = modeProperties.Checkpoints == null ? 1 : modeProperties.Checkpoints.Length + 1;
                    for (int j = 0; j < maxJ; j++)
                    {
                        var maxK = j == 0 ? modeProperties.StartStrawberries : modeProperties.Checkpoints[j - 1].Strawberries;
                        for (int k = 0; k < maxK; k++)
                        {
                            EntityData entityData = modeProperties.StrawberriesByCheckpoint[j, k];
                            if (entityData == null || entityData.Name != "strawberry")
                            {
                                continue;
                            }
                            strawberry = new EntityID(entityData.Level.Name, entityData.ID);
                            id = area.ID * OFFSET_LEVEL + i * OFFSET_SIDE + offset;
                            StrawberryMap.Add(id, strawberry);
                            StrawberryReverseMap.Add(strawberry.Key, id);
                            offset++;
                        }
                    }
                }
            }
        }

        private static EntityID? GetStrawberryEntityID(int area, int mode, int offset)
        {
            if (StrawberryMap == null)
            {
                BuildStrawberryMap();
            }

            int index = area * OFFSET_LEVEL + mode * OFFSET_SIDE + offset;
            if (StrawberryMap.ContainsKey(index))
            {
                return StrawberryMap[index];
            }

            return null;
        }

        public static int? GetStrawberryOffset(EntityID strawberry)
        {
            if (StrawberryMap == null)
            {
                BuildStrawberryMap();
            }

            if (StrawberryReverseMap.ContainsKey(strawberry.Key))
            {
                return StrawberryReverseMap[strawberry.Key];
            }
            return null;
        }

        private static EntityID GetTrapEntityID(TrapType offset)
        {
            string label;
            switch (offset) {
                case TrapType.THEO_CRYSTAL:
                    label = TrapType.THEO_CRYSTAL.ToString();
                    break;
                case TrapType.BADELINE_CHASERS:
                    label = TrapType.BADELINE_CHASERS.ToString();
                    break;
                case TrapType.SEEKER:
                    label = TrapType.SEEKER.ToString();
                    break;
                case TrapType.STAMINA:
                    label = TrapType.STAMINA.ToString();
                    break;
                default: 
                    throw new ArgumentOutOfRangeException($"Trap ({offset}) has not been implemented");
            }

            EntityID entityID = new EntityID(label, (int)offset);
            return entityID;
        }
    }
}
