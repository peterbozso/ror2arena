﻿using Arena.Logging;
using Arena.Managers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arena;

internal class Store : ILoggable
{
    private static readonly Lazy<Store> _instance = new(() => new Store());

    private readonly Dictionary<Type, ManagerBase> _managers = new();

    public static Store Instance => _instance.Value;

    private Store() { }

    public IEnumerable<string> GetStatus()
    {
        var status = new List<string> { $"Number of managers: {_managers.Count}" };

        if (_managers.Count > 0)
        {
            var managerNames = string.Join(", ", _managers.Select(kv => kv.Value.GetType().Name));
            status.Add($"Managers: {managerNames}");
        }

        return status;
    }

    public IEnumerable<ILoggable> GetLoggables() =>
        new[] { Instance }.Concat(_managers.Values.Cast<ILoggable>());

    public T Get<T>() where T : ManagerBase, new()
    {
        var type = typeof(T);

        if (_managers.ContainsKey(type))
        {
            return (T)_managers[type];
        }
        else
        {
            var manager = new T();
            _managers.Add(type, manager);
            return manager;
        }
    }

    public void CleanUp()
    {
        foreach (var manager in _managers.Values.OfType<ListeningManagerBase>())
        {
            manager.Stop();
        }

        _managers.Clear();
    }
}
