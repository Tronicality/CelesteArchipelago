using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.CelesteArchipelago {
    public class CelesteArchipelagoModuleSettings : EverestModuleSettings
    {
        [SettingMaxLength(30)]
        public string Name { get; set; } = "Madeline";
        public string Password { get; set; } = "";
        [SettingMaxLength(30)]
        public string Server { get; set; } = "archipelago.gg";
        public string Port { get; set; } = "38281";
        private bool _Chat = false;
        public bool Chat { 
            get => _Chat; 
            set {
                _Chat = value;
                if (value) {
                    ArchipelagoController.Instance.Init();
                } else {
                    ArchipelagoController.Instance.DeInit();
                }
            } 
        }
        // If adding new options under this comment, do not forget to change the DeathLinkMode insert value in the settingsUI
        private bool _DeathLink = false;
        [SettingInGame(true)]
        public bool DeathLink
        {
            get => _DeathLink;
            set
            {
                if (ArchipelagoController.Instance.DeathLinkService is not null)
                {
                    if (value) {
                        ArchipelagoController.Instance.DeathLinkService.EnableDeathLink();
                    }
                    else {
                        ArchipelagoController.Instance.DeathLinkService.DisableDeathLink();
                    }
                }
                _DeathLink = value;
            }
        }
        public DeathLinkMode DeathLinkMode = DeathLinkMode.Room;


        [DefaultButtonBinding(Buttons.Back, Keys.T)]
        public ButtonBinding ToggleChat { get; set; }
        [DefaultButtonBinding(Buttons.RightThumbstickUp, Keys.Q)]
        public ButtonBinding ScrollChatUp { get; set; }
        [DefaultButtonBinding(Buttons.RightThumbstickDown, Keys.Z)]
        public ButtonBinding ScrollChatDown { get; set; }
    }
}