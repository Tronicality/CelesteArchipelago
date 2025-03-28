namespace Celeste.Mod.CelesteArchipelago
{
    public class PatchedPlayer : IPatchable
    {
        public void Load()
        {
            Everest.Events.Player.OnSpawn += OnSpawn;
            Everest.Events.Player.OnDie += OnDie;
        }

        public void Unload()
        {
            Everest.Events.Player.OnSpawn -= OnSpawn;
            Everest.Events.Player.OnDie -= OnDie;
        }

        private static void OnSpawn(Player self)
        {            
            if (self.InControl && !self.SceneAs<Level>().InCutscene && !self.SceneAs<Level>().InCredits)
            {
                ArchipelagoController.Instance.trapManager.LoadTraps();
            }
        }

        private static void OnDie(Player self)
        {
            ArchipelagoController.Instance.trapManager.IncrementAllDeathCounts();
        }
    }
}