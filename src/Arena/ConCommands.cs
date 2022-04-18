using Arena.Logging;
using Arena.Managers;
using RoR2;
using UnityEngine.Networking;

namespace Arena;

internal static class ConCommands
{
    public const string EndArenaCommandName = "arena_end";

    [ConCommand(
        commandName = EndArenaCommandName,
        flags = ConVarFlags.None,
        helpText = "Ends the Arena event.")]
    public static void EndArenaEvent(ConCommandArgs _)
    {
        if (!NetworkServer.active)
        {
            Log.Debug($"{EndArenaCommandName}: Only the host can use this command.");
            return;
        }

        var arenaManager = Store.Instance.Get<ArenaManager>();

        if (!arenaManager.IsEventInProgress)
        {
            Log.Debug($"{EndArenaCommandName}: There's no arena event in progress.");
            return;
        }

        Log.Debug($"{EndArenaCommandName}: Ending arena event in response to console command...");

        arenaManager.EndArenaEvent();
    }
}
