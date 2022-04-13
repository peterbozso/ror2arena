namespace Arena.Logging;

internal class StatusLogger
{
    public void LogStatus()
    {
        Log.Debug(">>> ARENA MOD STATUS BEGIN");

        foreach (var loggable in Store.Instance.GetLoggables())
        {
            Log.Debug($"{loggable.GetType().Name} status:");

            foreach (var statusLine in loggable.GetStatus())
            {
                Log.Debug($"* {statusLine}");
            }
        }

        Log.Debug(">>> ARENA MOD STATUS END");
    }
}
