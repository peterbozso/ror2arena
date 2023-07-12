using System.Collections.Generic;
using Arena.Logging;
using Arena.Managers;
using RoR2;

namespace Arena;

internal static class ChaCommands
{
    public const string voteEndArenaCommand = "arena_skip";
    public static int voteEndCount = 0;

    //Collect a list of player objects who's voted
    public static List<string> votedPlayers = new List<string>();

    public static void ArenaEndVote(string playerName)
    {
        //Check if voteEndEnabled is enabled, if so return
        if (!ArenaPlugin.voteEndEnabled)
        {
            Log.Debug($"{voteEndArenaCommand}: Arena Voting is disabled");
            return;
        }

        //Check if the player has already voted
        if (votedPlayers.Contains(playerName))
        {
            Log.Debug($"{playerName}: has already voted to end the Arena.");
            return;
        }

        //Add the player to the list of voted players.
        votedPlayers.Add(playerName);
        voteEndCount++;

        //Divide the voteEndCount by the player count, if greater than votePercentNeeded, end the arena
        if (voteEndCount >= (int)(LocalUserManager.readOnlyLocalUsersList.Count * ArenaPlugin.votePercentNeeded))
        {
            Log.Debug($"{voteEndArenaCommand}: Users have voted to end the Arena.");
            Store.Instance.Get<ArenaManager>().VoteEndArenaEvent();
        }
    }
}
