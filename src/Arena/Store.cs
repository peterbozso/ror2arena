using Arena.Managers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arena;

internal class Store
{
    private static readonly Lazy<Dictionary<Type, ManagerBase>> _managers = new();

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
        foreach (var manager in _managers.Value.Values.OfType<ListeningManagerBase>())
        {
            manager.Stop();
        }

        Log.LogDebug($"Number of cleaned up managers: {_managers.Value.Count}");

        _managers.Value.Clear();
    }
}
