using System;

namespace Celeste.Mod.CelesteArchipelago
{
    public static class CelesteArchipelagoSettingsUI
    {
        public static void CreateMenu(TextMenu menu)
        {
            DeathlinkModeSelector(menu);
        }

        public static void DeathlinkModeSelector(TextMenu menu)
        {
            TextMenu.Option<DeathLinkMode> modeSelectionMenu = new("Deathlink Mode");

            modeSelectionMenu.Add(DeathLinkMode.Room.ToString(), DeathLinkMode.Room, true);
            foreach (DeathLinkMode mode in Enum.GetValues(typeof(DeathLinkMode)))
            {
                if (mode == DeathLinkMode.Room)
                {
                    continue;
                }

                bool selected = mode == CelesteArchipelagoModule.Settings.DeathLinkMode;
                modeSelectionMenu.Add(mode.ToString(), mode, selected);
            }

            modeSelectionMenu.Change(selectedMode => {
                CelesteArchipelagoModule.Settings.DeathLinkMode = selectedMode;
            });

            menu.Insert(menu.Items.Count - 2, modeSelectionMenu);
            modeSelectionMenu.AddDescription(menu, $"{DeathLinkMode.ChapterNoGoal}: {DeathLinkMode.Chapter} mode until goal level, then switches to {DeathLinkMode.Room} mode");
            modeSelectionMenu.AddDescription(menu, $"{DeathLinkMode.Chapter}: Restarts the whole chapter");
            modeSelectionMenu.AddDescription(menu, $"{DeathLinkMode.SubChapter}: Restarts to the most recently collected sub-chapter");
            modeSelectionMenu.AddDescription(menu, $"{DeathLinkMode.Room}: Normal Death");
        }
    }
}