﻿using System.Collections.Concurrent;

namespace Common.DataUtils;

/// <summary>
/// Cache IDs for a 1 or more database record types. Threadsafe.
/// </summary>
public class ConcurrentLookupDbIdsCache
{
    private ConcurrentDictionary<string, ConcurrentDictionary<string, int>> typeCache = new();
    public int? GetCachedIdForName<T>(string name) where T : class
    {
        lock (this)
        {
            var cacheName = typeof(T).FullName ?? "UnknownClass";

            var newDic = new ConcurrentDictionary<string, int>();
            typeCache.AddOrUpdate(cacheName, newDic, (index, oldVal) => typeCache[cacheName]);

            var cache = typeCache[cacheName];

            if (cache.ContainsKey(name))
            {
                return cache[name];
            }
            else return null;
        }
    }

    public void AddOrUpdateForName<T>(string name, int id)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        lock (this)
        {
            var cacheName = typeof(T).FullName ?? "UnknownClass";
            var newDic = new ConcurrentDictionary<string, int>();
            typeCache.AddOrUpdate(cacheName, newDic, (index, oldVal) => typeCache[cacheName]);

            var cache = typeCache[cacheName];

            cache.AddOrUpdate(name, id, (index, val) => val);
        }
    }
}
