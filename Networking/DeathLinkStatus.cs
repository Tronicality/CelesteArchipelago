namespace Celeste.Mod.CelesteArchipelago
{
    public enum DeathLinkStatus
    {
        // no deathlink has been received since the last time madeline completed dying from deathlink
        None,
        // a deathlink has been received but madeline has not started dying yet
        Pending,
        // a deathlink has been received and executed but madeline has not respawned yet
        Dying
    }
    public enum DeathLinkMode
    {
        // Normal Death
        Room,
        // Restarts sub chapter
        SubChapter,
        // Restarts chapter
        Chapter,
        // Restarts chapter however if on the goal level it will commence a room death
        ChapterNoGoal
    }
}