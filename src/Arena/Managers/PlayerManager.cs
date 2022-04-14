using Arena.Managers.Bases;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace Arena.Managers;

internal class PlayerManager : ManagerBase
{
    public override IEnumerable<string> GetStatus()
    {
        var alivePlayers = AlivePlayers;
        var alivePlayerNames = string.Join(
            ", ",
            alivePlayers.Select(player => player.master.GetBody().GetUserName()));
        var championName = ChampionName;

        return new string[]
        {
            $"Alive player count: { alivePlayers.Length }",
            $"Alive players: { (alivePlayerNames == string.Empty ? "none" : alivePlayerNames) }",
            $"Champion: { (championName == string.Empty ? "none" : championName) }."
        };
    }

    public NetworkUser[] AlivePlayers =>
        NetworkUser.readOnlyInstancesList.Where(user => IsAlive(user.master)).ToArray();

    public string ChampionName
    {
        get
        {
            var alivePlayers = AlivePlayers;

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
