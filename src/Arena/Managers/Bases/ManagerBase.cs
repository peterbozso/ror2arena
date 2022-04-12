using System.Collections.Generic;

namespace Arena.Managers.Bases;

internal abstract class ManagerBase
{
    public abstract IEnumerable<string> GetStatus();

    public void LogStatus()
    {
        Log.LogDebug($"{GetType().Name} status:");

        foreach (var statusLine in GetStatus())
        {
            Log.LogDebug($"* {statusLine}");
        }
    }
}
