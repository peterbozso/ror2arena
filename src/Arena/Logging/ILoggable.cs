using System.Collections.Generic;

namespace Arena.Logging;

internal interface ILoggable
{
    public IEnumerable<string> GetStatus();
}
