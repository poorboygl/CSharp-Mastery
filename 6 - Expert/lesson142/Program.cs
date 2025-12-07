// ======================================================================
// PROGRAM ENTRY – DOMAIN EVENTS DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== DOMAIN EVENT DISPATCHER DEMO ===");

        var dispatcher = new EventDispatcher();

        // REGISTER EVENT HANDLERS
        dispatcher.RegisterHandler<UserRegisteredEvent>(e =>
        {
            Console.WriteLine($"[Handler] User registered: {e.UserId}, Email = {e.Email}");
        });

        dispatcher.RegisterHandler<PasswordChangedEvent>(e =>
        {
            Console.WriteLine($"[Handler] User changed password: {e.UserId}");
        });

        // DEMO: CREATE USER
        Console.WriteLine("\n>>> Creating user...");
        var user = new User("john@example.com", dispatcher);

        // DEMO: CHANGE PASSWORD
        Console.WriteLine("\n>>> Changing password...");
        user.ChangePassword("new-password-123");

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// DOMAIN EVENT MARKER
// ======================================================================
public interface IDomainEvent { }


// ======================================================================
// DOMAIN EVENT DEFINITIONS
// ======================================================================
public class UserRegisteredEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserRegisteredEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}

public class PasswordChangedEvent : IDomainEvent
{
    public Guid UserId { get; }

    public PasswordChangedEvent(Guid userId)
    {
        UserId = userId;
    }
}


// ======================================================================
// DOMAIN EVENT DISPATCHER
// ======================================================================
public interface IEventDispatcher
{
    void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent;
}

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (_handlers.ContainsKey(eventType))
        {
            foreach (var handler in _handlers[eventType])
                ((Action<TEvent>)handler)(domainEvent);
        }
    }

    public void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.ContainsKey(eventType))
            _handlers[eventType] = new List<Delegate>();

        _handlers[eventType].Add(handler);
    }
}


// ======================================================================
// AGGREGATE ROOT: USER
// ======================================================================
public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }

    private readonly IEventDispatcher _eventDispatcher;

    public User(string email, IEventDispatcher eventDispatcher)
    {
        Id = Guid.NewGuid();
        Email = email;
        _eventDispatcher = eventDispatcher;

        // Publish domain event immediately
        _eventDispatcher.Publish(new UserRegisteredEvent(Id, Email));
    }

    public void ChangePassword(string newPassword)
    {
        Console.WriteLine("Password changed.");

        // Publish domain event
        _eventDispatcher.Publish(new PasswordChangedEvent(Id));
    }
}

/*
 === DOMAIN EVENT DISPATCHER DEMO ===

>>> Creating user...
[Handler] User registered: 193df4f9-0fa3-4ff1-9160-ff5cb060b80e, Email = john@example.com

>>> Changing password...
Password changed.
[Handler] User changed password: 193df4f9-0fa3-4ff1-9160-ff5cb060b80e

=== DONE ===
 */

/*
* 1.IDomainEvent: Acts as a marker interface for domain events.

* 2.EventDispatcher: Manages event publication and subscription, allowing handlers to listen for specific domain events.

* 3.User Entity: Encapsulates user-related actions and triggers domain events when actions occur, such as UserRegisteredEvent on registration and PasswordChangedEvent on password change.
 */


