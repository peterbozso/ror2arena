using Arena.Logging;
using Arena.Managers.Bases;
using RoR2;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Arena.Managers;

internal class FriendlyFireManager : ManagerBase
{
    private bool _isEnabled;

    public override IEnumerable<string> GetStatus() =>
        new[] { $"Friendly fire is { (_isEnabled ? "enabled" : "disabled") }." };

    public void EnableFriendlyFire()
    {
        if (_isEnabled)
        {
            return;
        }

        if (NetworkServer.active)
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);
        }

        _isEnabled = true;

        Log.Info("Friendly fire enabled.");
    }

    public void DisableFriendlyFire()
    {
        if (!_isEnabled)
        {
            return;
        }

        if (NetworkServer.active)
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
        }

        _isEnabled = false;

        Log.Info("Friendly fire disabled.");
    }
}
