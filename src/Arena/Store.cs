using Arena.Managers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arena;

internal class Store
{
    private static readonly Lazy<Dictionary<Type, ManagerBase>> _managers = new();

    public static void LogStatus()
    {
        LogManagers();

        foreach (var manager in _managers.Value)
        {
            manager.Value.LogStatus();
        }
    }

    public static T Get<T>() where T : ManagerBase, new()
    {
        var type = typeof(T);

        if (_managers.Value.ContainsKey(type))
        {
            return (T)_managers.Value[type];
        }
        else
        {
            var manager = new T();
            _managers.Value.Add(type, manager);

            Log.LogDebug($"Created: {type.Name}");

            return manager;
        }
    }

    public static void CleanUp()
    {
        Log.LogDebug($"Cleaning up.");
        LogManagers();

        foreach (var manager in _managers.Value.Values.OfType<ListeningManagerBase>())
        {
            manager.Stop();
        }

        _managers.Value.Clear();
    }

    private static void LogManagers()
    {
        Log.LogDebug($"{nameof(Store) } status:");
        Log.LogDebug($"* Number of managers: {_managers.Value.Count}");

        if (_managers.Value.Count > 0)
        {
            var managerNames = string.Join(", ", _managers.Value.Select(kv => kv.Value.GetType().Name));
            Log.LogDebug($"* Managers: {managerNames}");
        }
    }
}
