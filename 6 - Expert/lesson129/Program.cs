public class Program
{
    static void Main()
    {
        Console.WriteLine("=== DEMO EVENT BUS + NOTIFICATION SERVICE ===");

        var eventBus = new EventBus();
        var webSocket = new InMemoryWebSocketManager();
        var notificationService = new NotificationService(eventBus, webSocket);

        // Fake connection
        Guid userA = Guid.NewGuid();
        Guid userB = Guid.NewGuid();

        webSocket.AddConnection(userA, "conn-001");

        Console.WriteLine("\n--- Triggering Events ---");

        // User mentioned
        eventBus.Publish(new UserMentionedEvent(userA, "Hello @userA, welcome!"));

        // Post liked
        eventBus.Publish(new PostLikedEvent(userA, Guid.NewGuid()));

        // New follower (userB follows userA)
        eventBus.Publish(new NewFollowerEvent(userB, userA));

        // User offline case
        Console.WriteLine("\n--- User offline test ---");
        eventBus.Publish(new UserMentionedEvent(userB, "Hello @userB!"));

        Console.WriteLine("\n=== END OF PROGRAM ===");
        Console.ReadKey();
    }
}

// =============================================================
// Event Bus System
// =============================================================
public interface IEvent { }

public interface IEventPublisher
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}

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

// =============================================================
// Event Definitions
// =============================================================
public class UserMentionedEvent : IEvent
{
    public Guid MentionedUserId { get; }
    public string Message { get; }

    public UserMentionedEvent(Guid mentionedUserId, string message)
    {
        MentionedUserId = mentionedUserId;
        Message = message;
    }
}

public class PostLikedEvent : IEvent
{
    public Guid LikedUserId { get; }
    public Guid PostId { get; }

    public PostLikedEvent(Guid likedUserId, Guid postId)
    {
        LikedUserId = likedUserId;
        PostId = postId;
    }
}

public class NewFollowerEvent : IEvent
{
    public Guid FollowerId { get; }
    public Guid FollowedUserId { get; }

    public NewFollowerEvent(Guid followerId, Guid followedUserId)
    {
        FollowerId = followerId;
        FollowedUserId = followedUserId;
    }
}

// =============================================================
// WebSocket Manager
// =============================================================
public interface IWebSocketManager
{
    void AddConnection(Guid userId, string connectionId);
    void RemoveConnection(Guid userId);
    bool SendMessage(Guid userId, string message);
}

public class InMemoryWebSocketManager : IWebSocketManager
{
    private readonly Dictionary<Guid, string> _connections = new();

    public void AddConnection(Guid userId, string connectionId)
    {
        _connections[userId] = connectionId;
    }

    public void RemoveConnection(Guid userId)
    {
        _connections.Remove(userId);
    }

    public bool SendMessage(Guid userId, string message)
    {
        if (_connections.ContainsKey(userId))
        {
            Console.WriteLine($"Sent to {userId}: {message}");
            return true;
        }
        return false;
    }
}

// =============================================================
// Notification Service
// =============================================================
public class NotificationService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IWebSocketManager _webSocketManager;

    public NotificationService(IEventPublisher eventPublisher, IWebSocketManager webSocketManager)
    {
        _eventPublisher = eventPublisher;
        _webSocketManager = webSocketManager;

        _eventPublisher.Subscribe<UserMentionedEvent>(HandleUserMentioned);
        _eventPublisher.Subscribe<PostLikedEvent>(HandlePostLiked);
        _eventPublisher.Subscribe<NewFollowerEvent>(HandleNewFollower);
    }

    private void HandleUserMentioned(UserMentionedEvent @event)
    {
        var message = $"You were mentioned: {@event.Message}";
        SendNotification(@event.MentionedUserId, message);
    }

    private void HandlePostLiked(PostLikedEvent @event)
    {
        var message = "Your post was liked!";
        SendNotification(@event.LikedUserId, message);
    }

    private void HandleNewFollower(NewFollowerEvent @event)
    {
        var message = "You have a new follower!";
        SendNotification(@event.FollowedUserId, message);
    }

    private void SendNotification(Guid userId, string message)
    {
        if (!_webSocketManager.SendMessage(userId, message))
        {
            Console.WriteLine($"User {userId} is offline. Storing message for later.");
        }
    }
}

/*
 !=== DEMO EVENT BUS + NOTIFICATION SERVICE ===

--- Triggering Events ---
Sent to bb40eb69-32fa-49dc-b54a-fdadfb25342b: You were mentioned: Hello @userA, welcome!
Sent to bb40eb69-32fa-49dc-b54a-fdadfb25342b: Your post was liked!
Sent to bb40eb69-32fa-49dc-b54a-fdadfb25342b: You have a new follower!

--- User offline test ---
User 48491eae-d241-4efa-964a-881f4440d3fe is offline. Storing message for later.

=== END OF PROGRAM ===
 
 
 */

/*
* 1.EventBus:

    Manages subscriptions and triggers handlers when events occur.

* 2.Event Classes:

    UserMentionedEvent, PostLikedEvent, and NewFollowerEvent represent different types of notifications.

* 3.WebSocketManager:

    Tracks connections and simulates sending messages to online users.

* 4.NotificationService:

    Listens for events, formats notification messages, and sends them via WebSocketManager.
 
 */