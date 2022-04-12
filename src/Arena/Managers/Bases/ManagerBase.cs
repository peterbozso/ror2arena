using Arena.Logging;
using System.Collections.Generic;

namespace Arena.Managers.Bases;

internal abstract class ManagerBase : ILoggable
{
    public abstract IEnumerable<string> GetStatus();
}
