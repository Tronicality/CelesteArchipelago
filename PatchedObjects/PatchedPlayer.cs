using Microsoft.Xna.Framework;

namespace Celeste.Mod.CelesteArchipelago
{
    public class PatchedPlayer : IPatchable
    {
        public void Load()
        {
            On.Celeste.Player.Update += Update;
            Everest.Events.Player.OnSpawn += OnSpawn;
            Everest.Events.Player.OnDie += OnDie;
        }

        public void Unload()
        {
            On.Celeste.Player.Update -= Update;
            Everest.Events.Player.OnSpawn -= OnSpawn;
            Everest.Events.Player.OnDie -= OnDie;
        }

        private static void Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            if (ArchipelagoController.Instance.DeathLinkStatus == DeathLinkStatus.Pending && self.InControl)
            {
                ArchipelagoController.Instance.FlushDeathLinkMessage();
                self.Die(Vector2.Zero, true);
            }
            orig.Invoke(self);
        }

        private static void OnSpawn(Player player)
        {
            ArchipelagoController.Instance.trapManager.LoadTraps();

            if (ArchipelagoController.Instance.DeathLinkStatus == DeathLinkStatus.Dying)
            {
                ArchipelagoController.Instance.DeathLinkStatus = (ArchipelagoController.Instance.DeathLinkPool.Count > 0) ? DeathLinkStatus.Pending : DeathLinkStatus.None;
            }
        }

        private static void OnDie(Player player)
        {
            ArchipelagoController.Instance.trapManager.IncrementAllDeathCounts();
            ArchipelagoController.Instance.SendDeathLinkCallback();
            ArchipelagoController.Instance.DeathLinkStatus = DeathLinkStatus.Dying;
            ArchipelagoController.Instance.IsLocalDeath = true;
        }
    }
}