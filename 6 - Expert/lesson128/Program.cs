public class Program
{
    static void Main()
    {
        var eventBus = new EventBus();
        var cache = new InMemoryDistributedCache();

        var feedService = new FeedService(eventBus, cache);
        var postService = new PostService(eventBus);
        var followService = new FollowService(eventBus);

        var userA = Guid.NewGuid(); // user who posts
        var userB = Guid.NewGuid(); // user who follows

        Console.WriteLine("=== User A Creates a Post ===");
        postService.CreatePost(userA, "Hello world!");

        Console.WriteLine("\n=== User B Follows User A ===");
        followService.FollowUser(userB, userA);

        Console.WriteLine("\n=== Feed of User B ===");
        var feed = cache.GetFromCache(userB);
        foreach (var post in feed)
        {
            Console.WriteLine($" - {post}");
        }

        Console.ReadKey();
    }
}

// Define the IEvent and IEventPublisher interfaces
public interface IEvent { }

public interface IEventPublisher
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}

// In-memory event bus implementation for handling event publishing and subscribing
public class EventBus : IEventPublisher
{
    private readonly Dictionary<Type, List<Delegate>> _subscriptions = new();

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = @event.GetType();
        if (_subscriptions.ContainsKey(eventType))
        {
            foreach (var handler in _subscriptions[eventType])
            {
                handler.DynamicInvoke(@event);
            }
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (!_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType] = new List<Delegate>();
        }
        _subscriptions[eventType].Add(handler);
    }
}

// Domain events for post creation and user follow actions
public class PostCreatedEvent : IEvent
{
    public Guid PostId { get; }
    public Guid UserId { get; }
    public string Content { get; }

    public PostCreatedEvent(Guid postId, Guid userId, string content)
    {
        PostId = postId;
        UserId = userId;
        Content = content;
    }
}

public class UserFollowedEvent : IEvent
{
    public Guid FollowerId { get; }
    public Guid FollowedUserId { get; }

    public UserFollowedEvent(Guid followerId, Guid followedUserId)
    {
        FollowerId = followerId;
        FollowedUserId = followedUserId;
    }
}

// Interface and in-memory implementation of distributed cache
public interface IDistributedCache
{
    void AddToCache(Guid userId, string postContent);
    List<string> GetFromCache(Guid userId);
    void InvalidateCache(Guid userId);
}

public class InMemoryDistributedCache : IDistributedCache
{
    private readonly Dictionary<Guid, List<string>> _cache = new();

    public void AddToCache(Guid userId, string postContent)
    {
        if (!_cache.ContainsKey(userId))
            _cache[userId] = new List<string>();
        _cache[userId].Add(postContent);
    }

    public List<string> GetFromCache(Guid userId)
    {
        return _cache.ContainsKey(userId) ? _cache[userId] : new List<string>();
    }

    public void InvalidateCache(Guid userId)
    {
        _cache.Remove(userId);
    }
}

// Post service that publishes PostCreatedEvent when a new post is created
public class PostService
{
    private readonly IEventPublisher _eventPublisher;

    public PostService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public void CreatePost(Guid userId, string content)
    {
        var postId = Guid.NewGuid();
        _eventPublisher.Publish(new PostCreatedEvent(postId, userId, content));
    }
}

// Follow service that publishes UserFollowedEvent when a user follows another user
public class FollowService
{
    private readonly IEventPublisher _eventPublisher;

    public FollowService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public void FollowUser(Guid followerId, Guid followedUserId)
    {
        _eventPublisher.Publish(new UserFollowedEvent(followerId, followedUserId));
    }
}

// Feed service listens to events and manages the feed for followers
public class FeedService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IDistributedCache _cache;

    public FeedService(IEventPublisher eventPublisher, IDistributedCache cache)
    {
        _eventPublisher = eventPublisher;
        _cache = cache;
        _eventPublisher.Subscribe<PostCreatedEvent>(HandlePostCreated);
        _eventPublisher.Subscribe<UserFollowedEvent>(HandleUserFollowed);
    }

    private void HandlePostCreated(PostCreatedEvent @event)
    {
        _cache.AddToCache(@event.UserId, @event.Content);
    }

    private void HandleUserFollowed(UserFollowedEvent @event)
    {
        var posts = _cache.GetFromCache(@event.FollowedUserId);
        foreach (var post in posts)
        {
            _cache.AddToCache(@event.FollowerId, post);
        }
    }
}

/*
 Event-Driven Architecture (EDA) + Event Bus + Distributed Cache.
* 1.EventBus:

    Manages subscriptions and publishes events to all registered handlers.

* 2.PostService and FollowService:

    Publish events when a post is created or a user follows another user, respectively.

* 3.FeedService:

    Listens to PostCreatedEvent and UserFollowedEvent and updates followers’ feeds.

    Uses InMemoryDistributedCache to store recent posts for each user, allowing quick retrieval.
 */