using Arena.Logging;
using Arena.Managers;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine.Networking;

namespace Arena.Commands;

internal static class ConCommands
{
    public const string EndArenaCommandName = "arena_end";

    [ConCommand(
        commandName = EndArenaCommandName,
        flags = ConVarFlags.None,
        helpText = "Ends the Arena event.")]
    public static void EndArenaEvent(ConCommandArgs args)
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

        new EndArenaCommandNetMessage().Send(NetworkDestination.Clients | NetworkDestination.Server);
    }
}
