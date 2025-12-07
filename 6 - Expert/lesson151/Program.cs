// ======================================================================
// USER ACTIVITY TRACKING SYSTEM - CQRS + EVENT SOURCING DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        // ==================================================================
        // DEMO CHẠY THỬ
        // ==================================================================
        var eventStore = new EventStore();
        var handler = new ActivityCommandHandler(eventStore);
        var projection = new ActivityProjection();

        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();

        // User 1 login + xem trang
        handler.Handle(new LoginCommand(user1));
        handler.Handle(new ViewPageCommand(user1, "/home"));
        handler.Handle(new ViewPageCommand(user1, "/products"));
        handler.Handle(new LogoutCommand(user1));

        // User 2 login + xem trang
        handler.Handle(new LoginCommand(user2));
        handler.Handle(new ViewPageCommand(user2, "/home"));

        // Replay toàn bộ sự kiện vào Projection
        projection.Replay(eventStore.GetEvents());

        // In kết quả
        Console.WriteLine("===== ACTIVITY STATISTICS =====");
        Console.WriteLine($"Total Logins: {projection.TotalLogins}");
        Console.WriteLine($"Total Logouts: {projection.TotalLogouts}");
        Console.WriteLine($"Total Page Views: {projection.TotalPageViews}");
        Console.WriteLine($"User 1 Page Views: {projection.GetPageViewsForUser(user1)}");
        Console.WriteLine($"User 2 Page Views: {projection.GetPageViewsForUser(user2)}");

        Console.ReadKey();
    }
}

// ======================================================================
// COMMAND INTERFACES & COMMAND DEFINITIONS
// ======================================================================

public interface ICommand { }
public interface IEvent { }

public class LoginCommand : ICommand
{
    public Guid UserId { get; }
    public LoginCommand(Guid userId) => UserId = userId;
}

public class LogoutCommand : ICommand
{
    public Guid UserId { get; }
    public LogoutCommand(Guid userId) => UserId = userId;
}

public class ViewPageCommand : ICommand
{
    public Guid UserId { get; }
    public string PageUrl { get; }

    public ViewPageCommand(Guid userId, string pageUrl)
    {
        UserId = userId;
        PageUrl = pageUrl;
    }
}

// ======================================================================
// EVENT DEFINITIONS
// ======================================================================

public class UserLoggedInEvent : IEvent
{
    public Guid UserId { get; }
    public DateTime Timestamp { get; }

    public UserLoggedInEvent(Guid userId)
    {
        UserId = userId;
        Timestamp = DateTime.UtcNow;
    }
}

public class UserLoggedOutEvent : IEvent
{
    public Guid UserId { get; }
    public DateTime Timestamp { get; }

    public UserLoggedOutEvent(Guid userId)
    {
        UserId = userId;
        Timestamp = DateTime.UtcNow;
    }
}

public class PageViewedEvent : IEvent
{
    public Guid UserId { get; }
    public string PageUrl { get; }
    public DateTime Timestamp { get; }

    public PageViewedEvent(Guid userId, string pageUrl)
    {
        UserId = userId;
        PageUrl = pageUrl;
        Timestamp = DateTime.UtcNow;
    }
}

// ======================================================================
// EVENT STORE
// ======================================================================

public class EventStore
{
    private readonly List<IEvent> _events = new();
    public void SaveEvent(IEvent @event) => _events.Add(@event);
    public IEnumerable<IEvent> GetEvents() => _events;
}

// ======================================================================
// COMMAND HANDLER
// ======================================================================

public class ActivityCommandHandler
{
    private readonly EventStore _eventStore;

    public ActivityCommandHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public void Handle(LoginCommand command)
    {
        var ev = new UserLoggedInEvent(command.UserId);
        _eventStore.SaveEvent(ev);
    }

    public void Handle(LogoutCommand command)
    {
        var ev = new UserLoggedOutEvent(command.UserId);
        _eventStore.SaveEvent(ev);
    }

    public void Handle(ViewPageCommand command)
    {
        var ev = new PageViewedEvent(command.UserId, command.PageUrl);
        _eventStore.SaveEvent(ev);
    }
}

// ======================================================================
// PROJECTION (READ MODEL)
// ======================================================================

public class ActivityProjection
{
    public int TotalLogins { get; private set; }
    public int TotalLogouts { get; private set; }
    public int TotalPageViews { get; private set; }
    private readonly Dictionary<Guid, int> _userPageViews = new();

    public void Apply(IEvent @event)
    {
        switch (@event)
        {
            case UserLoggedInEvent:
                TotalLogins++;
                break;

            case UserLoggedOutEvent:
                TotalLogouts++;
                break;

            case PageViewedEvent pageViewed:
                TotalPageViews++;
                if (_userPageViews.ContainsKey(pageViewed.UserId))
                    _userPageViews[pageViewed.UserId]++;
                else
                    _userPageViews[pageViewed.UserId] = 1;
                break;
        }
    }

    public int GetPageViewsForUser(Guid userId) =>
        _userPageViews.TryGetValue(userId, out var count) ? count : 0;

    public void Replay(IEnumerable<IEvent> events)
    {
        foreach (var e in events)
            Apply(e);
    }
}

/*
 ===== ACTIVITY STATISTICS =====
Total Logins: 2
Total Logouts: 1
Total Page Views: 3
User 1 Page Views: 2
User 2 Page Views: 1
 */

/*
* 1.Command and Event Interfaces: ICommand and IEvent distinguish commands from events.

* 2.Commands and Events: LoginCommand, LogoutCommand, and ViewPageCommand represent actions, while events capture state changes.

* 3.EventStore: Stores all events, allowing for replay to rebuild the state.

* 4.ActivityProjection: Aggregates user activity data (e.g., total logins, logouts, and page views) and provides a queryable read model.
 
 */