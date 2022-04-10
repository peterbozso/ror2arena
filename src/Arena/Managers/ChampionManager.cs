using Arena.Managers.Bases;
using RoR2;
using System.Linq;

namespace Arena.Managers;

internal class ChampionManager : ManagerBase
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
        return bodyIsAlive || extraLifeCount > 0 || extraLifeVoidCount > 0;
    }
}
