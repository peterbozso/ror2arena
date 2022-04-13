using Arena.Logging;
using Arena.Managers;
using RoR2;
using FriendlyFireManager = Arena.Managers.FriendlyFireManager;

namespace Arena;

internal class Commands
{
    private const string EndArenaCommandName = "arena_end";

    [ConCommand(commandName = EndArenaCommandName, flags = ConVarFlags.None, helpText = "Ends the Arena event.")]
    public static void EndArenaEvent(ConCommandArgs args)
    {
        if (!Store.Instance.Get<ArenaManager>().IsEventInProgress)
        {
            Log.Debug($"{EndArenaCommandName}: There's no arena event in progress.");
            return;
        }

        Log.Debug($"{EndArenaCommandName}: Ending arena event in response to console command.");

        Store.Instance.Get<ClockManager>().ResumeClock();
        Store.Instance.Get<FriendlyFireManager>().DisableFriendlyFire();
        Store.Instance.Get<PortalManager>().EnableAllPortals();
        Store.Instance.Get<DeathManager>().Stop();
    }
}
