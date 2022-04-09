using RoR2;
using System.Linq;

namespace Arena.Managers;

public class ChampionManager
{
    public string ChampionName
    {
        get
        {
            var players = PlayerCharacterMasterController.instances;

            var alivePlayers = players.Where(player => IsAlive(player.master)).ToArray();

            return alivePlayers.Length == 1
                ? alivePlayers[0].master.GetBody().GetUserName()
                : string.Empty;
        }
    }

    private static bool IsAlive(CharacterMaster player)
    {
        var body = player.GetBody();
        var bodyIsAlive = body && body.healthComponent.alive;
        var extraLifeCount = player.inventory.GetItemCount(RoR2Content.Items.ExtraLife);
        var extraLifeVoidCount = player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid);
        var isAlive = bodyIsAlive || extraLifeCount > 0 || extraLifeVoidCount > 0;

        Log.LogDebug(player.GetBody().GetUserName());
        Log.LogDebug("body is " + (bodyIsAlive ? "alive" : "dead"));
        Log.LogDebug("ExtraLife count: " + player.inventory.GetItemCount(RoR2Content.Items.ExtraLife));
        Log.LogDebug("ExtraLifeVoid count: " + player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid));
        Log.LogDebug("summary: player is " + (isAlive ? "alive" : "dead"));

        return isAlive;
    }
}
