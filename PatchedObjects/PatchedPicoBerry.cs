namespace Celeste.Mod.CelesteArchipelago
{
    public class PatchedPicoBerry : IPatchable
    {
        public void Load()
        {
            On.Celeste.Pico8.Classic.fruit.update += BerryUpdate;
            On.Celeste.Pico8.Classic.fly_fruit.update += FlyBerryUpdate;
        }

        public void Unload()
        {
            On.Celeste.Pico8.Classic.fruit.update -= BerryUpdate;
            On.Celeste.Pico8.Classic.fly_fruit.update -= FlyBerryUpdate;
        }

        private void BerryUpdate(On.Celeste.Pico8.Classic.fruit.orig_update orig, Pico8.Classic.fruit self)
        {
            Logger.Log(LogLevel.Debug, "CelesteArchipelago", $"Berry: {self.x}");
            if (self.check<Pico8.Classic.player>(0, 0))
            {
                // Player has collided with Fruit
                /*
                // Unique Entity ID could be x coord * type (fruit = 1),
                // Problem is that chest berries (seems to only be them) have a small range of x coords of +-5
                // These can be seen when dying and recollecting (especially with chest berries)
                // If CelestePublicizer is allowed then unique IDs can be created (1 + G.level_index())

                switch (self.x * 1)
                {
                    case 12:
                        // Berry 1
                        break;
                    case 8:
                        // Berry 2
                        break;
                    case 115:
                        // Chest Berry 4
                        break;
                    case 60:
                        // Breakable Berry 6
                        break;
                    case 108:
                        // Chest Berry 8
                        break;
                    case 16:
                        // Berry 9
                        break;
                    case 53:
                        // Chest Berry 10
                        break;
                    case 4:
                        // Breakable Berry 11
                        break;
                    case 85:
                        // Chest Berry 12
                        break;
                    case 44:
                        // Chest Berry 14
                        break;
                    case 116:
                        // Chest Berry 15
                        break;
                    case 61:
                        // Chest Berry 16
                        break;
                    case 0:
                        // Berry 17
                        break;
                    case 59:
                        // Chest Berry 18
                        break;
                }
                */
                ///ArchipelagoController.Instance.ProgressionSystem.OnCollectedClient(SaveData.Instance.CurrentSession_Safe.Area, CollectableType.PICO_BERRY);
            }

            orig(self);
        }

        private void FlyBerryUpdate(On.Celeste.Pico8.Classic.fly_fruit.orig_update orig, Pico8.Classic.fly_fruit self)
        {
            if (self.check<Pico8.Classic.player>(0, 0))
            {
                // Player has collided with Fly Fruit
                Logger.Log(LogLevel.Debug, "CelesteArchipelago", $"Fly Berry: {self.x}");
                /*
                // Entity ID could be x coord * type (fly_fruit = 2), if celestepublicizer is allowed then IDs can be created
                switch (self.x * 2)
                {
                    case 16:
                        // Berry 3
                        break;
                    case 80:
                        // Berry 5
                        break;
                    case 96:
                        // Berry 7
                        break;
                    case 112:
                        // Berry 13
                        break;
                }
                */
                //ArchipelagoController.Instance.ProgressionSystem.OnCollectedClient(SaveData.Instance.CurrentSession_Safe.Area, CollectableType.PICO_BERRY);
            }

            orig(self);
        }
    }
}