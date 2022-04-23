using Arena.Managers.Bases;
using Arena.Models;
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
            alivePlayers.Select(player => player.GetBody().GetUserName()));
        var champion = Champion;

        return new string[]
        {
            $"Alive player count: { alivePlayers.Length }",
            $"Alive players: { (alivePlayerNames == string.Empty ? "none" : alivePlayerNames) }",
            $"Champion: { (champion == null ? "none" : champion.Name) }."
        };
    }

    public Champion Champion
    {
        get
        {
            var alivePlayers = AlivePlayers;

            return alivePlayers.Length == 1
                ? new Champion(alivePlayers[0])
                : null;
        }
    }

    public int AlivePlayerCount => AlivePlayers.Length;

    private static CharacterMaster[] AlivePlayers =>
        Server.AllPlayers.Where(user => IsAlive(user)).ToArray();

    private static bool IsAlive(CharacterMaster player)
    {
        var body = player.GetBody();
        var bodyIsAlive = body && body.healthComponent.alive;
        var extraLifeCount = player.inventory.GetItemCount(RoR2Content.Items.ExtraLife);
        var extraLifeVoidCount = player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid);
        return bodyIsAlive || extraLifeCount > 0 || extraLifeVoidCount > 0;
    }
}
