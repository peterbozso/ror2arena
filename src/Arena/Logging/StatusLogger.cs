using System.Collections.Generic;

namespace Arena.Logging;

internal class StatusLogger
{
    public void LogStatus(IEnumerable<ILoggable> loggables)
    {
        foreach (var loggable in loggables)
        {
            Log.LogDebug($"{loggable.GetType().Name} status:");

            foreach (var statusLine in loggable.GetStatus())
            {
                Log.LogDebug($"* {statusLine}");
            }
        }
    }
}
