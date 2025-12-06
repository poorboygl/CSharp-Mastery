using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        // ======================================================================
        // DEMO: HASH-BASED SHARDING WITH QUERY ROUTER
        // ======================================================================

        // Tạo 3 shard database
        var shards = new List<Shard>
        {
            new Shard(0),
            new Shard(1),
            new Shard(2)
        };

        // Khởi tạo shard manager (hash-based)
        IShardManager shardManager = new HashBasedShardManager(shards);

        // Router để phân tích userId -> shard
        var router = new QueryRouter(shardManager);

        // Thực hiện insert dữ liệu
        router.Insert(101, "User 101 profile data");
        router.Insert(202, "User 202 profile data");
        router.Insert(303, "User 303 profile data");

        // Query dữ liệu
        Console.WriteLine(router.Query(101));
        Console.WriteLine(router.Query(202));
        Console.WriteLine(router.Query(303));

        Console.ReadKey();
    }
}

// ======================================================================
// INTERFACE: SHARD MANAGER
// ======================================================================
public interface IShardManager
{
    Shard GetShard(int userId);
}

// ======================================================================
// SHARD CLASS (MỖI SHARD NHƯ 1 DATABASE)
// ======================================================================
public class Shard
{
    private readonly Dictionary<int, string> _data = new();
    public int ShardId { get; }

    public Shard(int shardId)
    {
        ShardId = shardId;
    }

    public void Insert(int userId, string data)
    {
        _data[userId] = data;
        Console.WriteLine($"Data inserted for User {userId} in Shard {ShardId}.");
    }

    public string Query(int userId)
    {
        return _data.TryGetValue(userId, out var result)
            ? result
            : "No data found";
    }
}

// ======================================================================
// HASH-BASED SHARD MANAGER (userId % shardCount)
// ======================================================================
public class HashBasedShardManager : IShardManager
{
    private readonly List<Shard> _shards;

    public HashBasedShardManager(List<Shard> shards)
    {
        _shards = shards;
    }

    public Shard GetShard(int userId)
    {
        int shardIndex = userId % _shards.Count;
        return _shards[shardIndex];
    }
}

// ======================================================================
// QUERY ROUTER (ĐỊNH TUYẾN REQUEST ĐẾN SHARD PHÙ HỢP)
// ======================================================================
public class QueryRouter
{
    private readonly IShardManager _shardManager;

    public QueryRouter(IShardManager shardManager)
    {
        _shardManager = shardManager;
    }

    public void Insert(int userId, string data)
    {
        var shard = _shardManager.GetShard(userId);
        shard.Insert(userId, data);
    }

    public string Query(int userId)
    {
        var shard = _shardManager.GetShard(userId);
        return shard.Query(userId);
    }
}
/*
Data inserted for User 101 in Shard 2.
Data inserted for User 202 in Shard 1.
Data inserted for User 303 in Shard 0.
User 101 profile data
User 202 profile data
User 303 profile data
 */

/*
* 1.Shard: Represents a single shard, simulating a database instance that stores user data based on user ID.

* 2.HashBasedShardManager: Implements hash-based sharding. It calculates the shard index by applying userId % shardCount.

* 3.QueryRouter: Routes data operations (insertions and queries) to the correct shard by calling GetShard from ShardManager.
 */