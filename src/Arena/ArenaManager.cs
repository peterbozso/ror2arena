using RoR2;
using System.Linq;

namespace Arena;

internal static class ArenaManager
{
    public static class Clock
    {
        public static void Pause()
        {
            Stage.instance.sceneDef.sceneType = SceneType.Intermission;
            Log.LogMessage("Clock paused.");
        }

        public static void Resume()
        {
            Stage.instance.sceneDef.sceneType = SceneType.Stage;
            Log.LogMessage("Clock resumed.");
        }
    }

    public static class FriendlyFire
    {
        public static void Enable()
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);
            Log.LogMessage("Friendly fire enabled.");
        }

        public static void Disable()
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
            Log.LogMessage("Friendly fire disabled.");
        }
    }

    public static class Champion
    {
        public static string Name
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
            return (body && body.healthComponent.alive)
                || player.inventory.GetItemCount(RoR2Content.Items.ExtraLife) > 0
                || player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) > 0;
        }
    }
}
