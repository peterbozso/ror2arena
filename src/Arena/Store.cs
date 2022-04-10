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
        if (_managers.Value.ContainsKey(typeof(T)))
        {
            return (T)_managers.Value[typeof(T)];
        }
        else
        {
            var managerType = typeof(T);
            var manager = new T();
            _managers.Value.Add(managerType, manager);

            Log.LogDebug($"Created: {managerType.Name}");

            return manager;
        }
    }

    public static void DestroyAll()
    {
        foreach (var manager in _managers.Value.Values.OfType<ListeningManagerBase>())
        {
            manager.StopIfListening();
        }

        Log.LogDebug($"Number of destroyed managers: {_managers.Value.Count}");

        _managers.Value.Clear();
    }
}
