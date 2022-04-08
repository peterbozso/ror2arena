using RoR2;
using System.Linq;

namespace Arena.Managers;

public class Champion
{
    public string Name
    {
        get
        {
            var players = PlayerCharacterMasterController.instances;
            var alivePlayers = players.Where(player => IsAlive(player.master)).ToArray();

            if (alivePlayers.Length == 1)
            {
                return alivePlayers[0].master.GetBody().GetUserName();
            }

            return string.Empty;
        }
    }

    private static bool IsAlive(CharacterMaster player)
    {
        var body = player.GetBody();
        return body && body.healthComponent.alive
            || player.inventory.GetItemCount(RoR2Content.Items.ExtraLife) > 0
            || player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) > 0;
    }
}
