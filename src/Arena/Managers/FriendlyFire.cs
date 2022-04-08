using RoR2;

namespace Arena.Managers;

public class FriendlyFire
{
    public void Enable()
    {
        RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);
        Log.LogMessage("Friendly fire enabled.");
    }

    public void Disable()
    {
        RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
        Log.LogMessage("Friendly fire disabled.");
    }
}
