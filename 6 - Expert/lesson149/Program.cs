// ======================================================================
// PROGRAM DEMO – DOMAIN EVENTS (Member Registration & Upgrade)
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== DOMAIN EVENTS DEMO ===");

        var dispatcher = new EventDispatcher();

        // Đăng ký (subscribe) các event handler
        dispatcher.Subscribe<MemberRegisteredEvent>(e =>
            new SendWelcomeEmailHandler().Handle(e));

        dispatcher.Subscribe<MembershipUpgradedEvent>(e =>
            new UpdateRewardsHandler().Handle(e));

        // Tạo member + fire event
        var member = new Member("john@domain.com", dispatcher);

        Console.WriteLine("\n--- Register Member ---");
        member.RegisterMember();

        Console.WriteLine("\n--- Upgrade Membership ---");
        member.UpgradeMembership();

        Console.WriteLine("==============================");
        Console.ReadKey();
    }
}

// ======================================================================
// DOMAIN EVENTS
// ======================================================================
public interface IDomainEvent { }

public class MemberRegisteredEvent : IDomainEvent
{
    public Guid MemberId { get; }
    public string Email { get; }

    public MemberRegisteredEvent(Guid memberId, string email)
    {
        MemberId = memberId;
        Email = email;
    }
}

public class MembershipUpgradedEvent : IDomainEvent
{
    public Guid MemberId { get; }

    public MembershipUpgradedEvent(Guid memberId)
    {
        MemberId = memberId;
    }
}

// ======================================================================
// EVENT HANDLERS
// ======================================================================
public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
    void Handle(TEvent @event);
}

public class SendWelcomeEmailHandler : IEventHandler<MemberRegisteredEvent>
{
    public void Handle(MemberRegisteredEvent @event)
    {
        Console.WriteLine($"[Email] Welcome email sent to {@event.Email}");
    }
}

public class UpdateRewardsHandler : IEventHandler<MembershipUpgradedEvent>
{
    public void Handle(MembershipUpgradedEvent @event)
    {
        Console.WriteLine($"[Rewards] Updated rewards for member {@event.MemberId}");
    }
}

// ======================================================================
// EVENT DISPATCHER (in-memory)
// ======================================================================
public class EventDispatcher
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (_handlers.ContainsKey(eventType))
        {
            foreach (var handler in _handlers[eventType])
            {
                ((Action<TEvent>)handler)(@event);
            }
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Delegate>();
        }

        _handlers[eventType].Add(handler);
    }
}

// ======================================================================
// MEMBER AGGREGATE ROOT – triggers domain events
// ======================================================================
public class Member
{
    public Guid Id { get; }
    public string Email { get; private set; }
    private readonly EventDispatcher _eventDispatcher;

    public Member(string email, EventDispatcher eventDispatcher)
    {
        Id = Guid.NewGuid();
        Email = email;
        _eventDispatcher = eventDispatcher;
    }

    public void RegisterMember()
    {
        var memberRegisteredEvent = new MemberRegisteredEvent(Id, Email);
        _eventDispatcher.Publish(memberRegisteredEvent);
    }

    public void UpgradeMembership()
    {
        var membershipUpgradedEvent = new MembershipUpgradedEvent(Id);
        _eventDispatcher.Publish(membershipUpgradedEvent);
    }
}

/*
 === DOMAIN EVENTS DEMO ===

--- Register Member ---
[Email] Welcome email sent to john@domain.com

--- Upgrade Membership ---
[Rewards] Updated rewards for member a3df716d-5a9f-4fe2-982b-a391c90c4fca
==============================
 */

/*
* 1.Event and Handler Interfaces: The IDomainEvent and IEventHandler<TEvent> interfaces are used to identify domain events and their handlers.

* 2.Event Classes: MemberRegisteredEvent and MembershipUpgradedEvent represent key changes in the Member entity.

* 3.Handlers: SendWelcomeEmailHandler and UpdateRewardsHandler react to events by performing specific actions.

* 4.EventDispatcher: Manages the subscription of handlers and the publication of events to all relevant handlers.

* 5.Member Class: Publishes events (e.g., MemberRegisteredEvent) without directly invoking handlers, maintaining decoupling.
 */