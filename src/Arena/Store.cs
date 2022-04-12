using Arena.Logging;
using Arena.Managers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arena;

internal class Store : ILoggable
{
    private static readonly Lazy<Store> _instance = new();

    private static Store Instance { get { return _instance.Value; } }

    private Dictionary<Type, ManagerBase> Managers { get; } = new();

    private Store() { }

    public static IEnumerable<ILoggable> GetLoggables() =>
        Instance.Managers.OfType<ILoggable>().ToList().Concat(new[] { Instance });

    public static T Get<T>() where T : ManagerBase, new()
    {
        var type = typeof(T);

        if (Instance.Managers.ContainsKey(type))
        {
            return (T)Instance.Managers[type];
        }
        else
        {
            var manager = new T();
            Instance.Managers.Add(type, manager);

            Log.LogDebug($"Created: {type.Name}");

            return manager;
        }
    }

    public static void CleanUp()
    {
        foreach (var manager in Instance.Managers.Values.OfType<ListeningManagerBase>())
        {
            manager.Stop();
        }

        Instance.Managers.Clear();

        Log.LogDebug($"Cleaned up.");
    }

    public IEnumerable<string> GetStatus()
    {
        var status = new List<string> { $"Number of managers: {Instance.Managers.Count}" };

        if (Instance.Managers.Count > 0)
        {
            var managerNames = string.Join(", ", Instance.Managers.Select(kv => kv.Value.GetType().Name));
            status.Add($"Managers: {managerNames}");
        }

        return status;
    }
}
