using RoR2;

namespace Arena.Managers;

internal class FriendlyFireManager : ManagerBase
{
    public void EnableFriendlyFire()
    {
        RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);
        Log.LogMessage("Friendly fire enabled.");
    }

    public void DisableFriendlyFire()
    {
        RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
        Log.LogMessage("Friendly fire disabled.");
    }
}
