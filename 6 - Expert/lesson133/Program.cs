using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Distributed Cache Load Balancer Demo ===\n");

        var node1 = new CacheNode("Node-1");
        var node2 = new CacheNode("Node-2");
        var node3 = new CacheNode("Node-3");

        var loadBalancer = new CacheLoadBalancer(new List<ICache> { node1, node2, node3 });

        // ======================================================================
        // DEMO: SET DATA
        // ======================================================================
        loadBalancer.Set("User:1", "Vu Nguyen");
        loadBalancer.Set("User:2", "Alice");
        loadBalancer.Set("User:3", "Bob");

        Console.WriteLine("\n--- GET DATA ---");

        // ======================================================================
        // DEMO: GET DATA
        // ======================================================================
        Console.WriteLine($"Get User:1 --> {loadBalancer.Get("User:1")}");
        Console.WriteLine($"Get User:2 --> {loadBalancer.Get("User:2")}");
        Console.WriteLine($"Get User:3 --> {loadBalancer.Get("User:3")}");

        // ======================================================================
        // DEMO: INVALIDATE
        // ======================================================================
        Console.WriteLine("\n--- INVALIDATE User:2 ---");
        loadBalancer.Invalidate("User:2");

        Console.WriteLine("\nGet User:2 again --> " + loadBalancer.Get("User:2"));

        Console.WriteLine("\n=== DEMO END ===");
        Console.ReadKey();
    }
}

public interface ICache
{
    void Set(string key, object value);
    object Get(string key);
    void Invalidate(string key);
}

// ======================================================================
// CACHE NODE – REPRESENTS A SINGLE CACHE SERVER
// ======================================================================
public class CacheNode : ICache
{
    private readonly Dictionary<string, object> _cache = new();
    private readonly string _nodeName;

    public CacheNode(string nodeName)
    {
        _nodeName = nodeName;
    }

    public void Set(string key, object value)
    {
        _cache[key] = value;
        Console.WriteLine($"[{_nodeName}] Set {key}");
    }

    public object Get(string key)
    {
        _cache.TryGetValue(key, out var value);
        Console.WriteLine($"[{_nodeName}] Get {key} --> {(value != null ? "HIT" : "MISS")}");
        return value;
    }

    public void Invalidate(string key)
    {
        if (_cache.ContainsKey(key))
        {
            _cache.Remove(key);
            Console.WriteLine($"[{_nodeName}] Invalidate {key}");
        }
    }
}

// ======================================================================
// LOAD BALANCER – DISTRIBUTES KEYS ROUND-ROBIN
// ======================================================================
public class CacheLoadBalancer
{
    private readonly List<ICache> _cacheNodes;
    private int _currentNodeIndex;

    public CacheLoadBalancer(List<ICache> cacheNodes)
    {
        _cacheNodes = cacheNodes;
        _currentNodeIndex = 0;
    }

    public void Set(string key, object value)
    {
        var node = GetNextNode();
        node.Set(key, value);
    }

    public object Get(string key)
    {
        foreach (var node in _cacheNodes)
        {
            var value = node.Get(key);
            if (value != null)
                return value;
        }
        return null;
    }

    public void Invalidate(string key)
    {
        foreach (var node in _cacheNodes)
        {
            node.Invalidate(key);
        }
    }

    private ICache GetNextNode()
    {
        var node = _cacheNodes[_currentNodeIndex];
        _currentNodeIndex = (_currentNodeIndex + 1) % _cacheNodes.Count;
        return node;
    }
}

/*
 !=== Distributed Cache Load Balancer Demo ===

[Node-1] Set User:1
[Node-2] Set User:2
[Node-3] Set User:3

--- GET DATA ---
[Node-1] Get User:1 --> HIT
Get User:1 --> Vu Nguyen
[Node-1] Get User:2 --> MISS
[Node-2] Get User:2 --> HIT
Get User:2 --> Alice
[Node-1] Get User:3 --> MISS
[Node-2] Get User:3 --> MISS
[Node-3] Get User:3 --> HIT
Get User:3 --> Bob

--- INVALIDATE User:2 ---
[Node-2] Invalidate User:2
[Node-1] Get User:2 --> MISS
[Node-2] Get User:2 --> MISS
[Node-3] Get User:2 --> MISS

Get User:2 again -->

=== DEMO END ===
 
 */

/*
* 1.CacheNode: Each cache node simulates local cache storage using a dictionary. Methods include Set, Get, and Invalidate.

* 2.CacheLoadBalancer: Distributes cache requests across multiple nodes in a round-robin manner. Invalidate broadcasts the invalidation request to all nodes to ensure data consistency.
 */