using Arena.Managers;
using System;
using System.Collections.Generic;

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
            var manager = new T();
            _managers.Value.Add(typeof(T), manager);
            return manager;
        }
    }

    public static void DestroyAll()
    {
        foreach (var manager in _managers.Value.Values)
        {
            manager.Destroy();
        }

        _managers.Value.Clear();
    }
}
