public class Program
{
    static void Main()
    {
        Console.WriteLine("===== SIMPLE CACHE MANAGER (Singleton + Generic Cache) =====\n");

        var cache = CacheManager.Instance;

        // Add cache item with 3-second expiration
        cache.Add("username", "JohnDoe", TimeSpan.FromSeconds(3));
        cache.Add("score", 1500, TimeSpan.FromSeconds(5));

        Console.WriteLine("Stored values:");
        Console.WriteLine($"username: {cache.Get<string>("username")}");
        Console.WriteLine($"score: {cache.Get<int>("score")}");

        Console.WriteLine("\nWaiting 4 seconds for 'username' to expire...");
        Thread.Sleep(4000);

        Console.WriteLine("\nAfter expiration:");
        Console.WriteLine($"username: {cache.Get<string>("username")}");
        Console.WriteLine($"score: {cache.Get<int>("score")}");

        Console.ReadKey();
    }
}


public class CacheItem<T>
{
    public T Value { get; set; }
    public DateTime ExpirationTime { get; set; }
}

public class CacheManager
{
    private static CacheManager _instance;
    private readonly Dictionary<string, CacheItem<object>> _cache = new();

    private CacheManager() { }

    public static CacheManager Instance => _instance ??= new CacheManager();

    public void Add<T>(string key, T value, TimeSpan expiration)
    {
        var cacheItem = new CacheItem<object>
        {
            Value = value,
            ExpirationTime = DateTime.UtcNow.Add(expiration)
        };
        _cache[key] = cacheItem;
    }

    public T Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var cacheItem))
        {
            if (DateTime.UtcNow <= cacheItem.ExpirationTime)
            {
                return (T)cacheItem.Value;
            }
            else
            {
                _cache.Remove(key); // Remove expired item
            }
        }
        return default;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}

/*
* 1.Singleton Pattern:

    CacheManager.Instance returns a single instance of CacheManager.

* 2.Cache with Expiration:

    Add stores CacheItem<object> with a calculated expiration time.

    Get retrieves items if they are still valid, removing expired items.

* 3.Generics for Flexible Value Types:

    Cache values are stored as object, allowing the cache to store any type. Casting in Get<T> provides type safety.
 */