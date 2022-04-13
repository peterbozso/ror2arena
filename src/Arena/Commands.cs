using Arena.Logging;
using Arena.Managers;
using RoR2;

namespace Arena;

internal class Commands
{
    private const string EndArenaCommandName = "arena_end";

    [ConCommand(commandName = EndArenaCommandName, flags = ConVarFlags.None, helpText = "Ends the Arena event.")]
    public static void EndArenaEvent(ConCommandArgs args)
    {
        var arenaManager = Store.Instance.Get<ArenaManager>();

        if (!arenaManager.IsEventInProgress)
        {
            Log.Debug($"{EndArenaCommandName}: There's no arena event in progress.");
            return;
        }

        Log.Debug($"{EndArenaCommandName}: Ending arena event in response to console command...");

        arenaManager.EndArenaEventManually();

        Log.Info("Arena event ended.");
    }
}
