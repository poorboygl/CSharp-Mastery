// ======================================================================
// PROGRAM DEMO – SAGA PATTERN (EVENT-DRIVEN)
// ======================================================================
public class Program
{
    static void Main()
    {
        var eventBus = new EventBus();
        var saga = new OrderSaga(eventBus);

        // ĐĂNG KÝ SUBSCRIBERS
        eventBus.Subscribe<OrderPlacedEvent>(saga.HandleOrderPlaced);
        eventBus.Subscribe<InventoryReservedEvent>(saga.HandleInventoryReserved);
        eventBus.Subscribe<OrderCancelledEvent>(saga.HandleOrderCancelled);

        Console.WriteLine("=== DEMO SAGA START ===");
        var orderId = Guid.NewGuid();

        saga.StartSaga(orderId);

        Console.WriteLine("=== SAGA COMPLETED ===");

        Console.ReadKey();
    }
}

// ======================================================================
// EVENT MARKER INTERFACE
// ======================================================================
public interface IEvent { }

// ======================================================================
// EVENT DEFINITIONS
// ======================================================================
public class OrderPlacedEvent : IEvent
{
    public Guid OrderId { get; }
    public OrderPlacedEvent(Guid orderId) => OrderId = orderId;
}

public class InventoryReservedEvent : IEvent
{
    public Guid OrderId { get; }
    public InventoryReservedEvent(Guid orderId) => OrderId = orderId;
}

public class PaymentCompletedEvent : IEvent
{
    public Guid OrderId { get; }
    public PaymentCompletedEvent(Guid orderId) => OrderId = orderId;
}

public class OrderCancelledEvent : IEvent
{
    public Guid OrderId { get; }
    public OrderCancelledEvent(Guid orderId) => OrderId = orderId;
}

// ======================================================================
// SIMPLE EVENT BUS
// ======================================================================
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var handler in _subscribers[eventType])
                ((Action<TEvent>)handler)(@event);
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (!_subscribers.ContainsKey(eventType))
            _subscribers[eventType] = new List<Delegate>();

        _subscribers[eventType].Add(handler);
    }
}

// ======================================================================
// ORDER SAGA (PROCESS MANAGER)
// ======================================================================
public class OrderSaga
{
    private readonly EventBus _eventBus;

    public OrderSaga(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    // BẮT ĐẦU SAGA
    public void StartSaga(Guid orderId)
    {
        Console.WriteLine($"Saga started for order {orderId}");
        _eventBus.Publish(new OrderPlacedEvent(orderId));
    }

    // STEP 1 → handle OrderPlaced
    public void HandleOrderPlaced(OrderPlacedEvent @event)
    {
        Console.WriteLine($"OrderPlacedEvent received --> reserving inventory...");
        _eventBus.Publish(new InventoryReservedEvent(@event.OrderId));
    }

    // STEP 2 → handle InventoryReserved
    public void HandleInventoryReserved(InventoryReservedEvent @event)
    {
        Console.WriteLine($"InventoryReservedEvent received --> processing payment...");
        _eventBus.Publish(new PaymentCompletedEvent(@event.OrderId));
    }

    // OPTIONAL: CANCEL HANDLER
    public void HandleOrderCancelled(OrderCancelledEvent @event)
    {
        Console.WriteLine($"Order {@event.OrderId} cancelled --> rollback completed.");
    }
}

/*
 === DEMO SAGA START ===
Saga started for order 6cac1eec-fa87-4a93-a8d1-14ad71f31b2f
OrderPlacedEvent received --> reserving inventory...
InventoryReservedEvent received --> processing payment...
=== SAGA COMPLETED ===

 */

/*
* 1.Event Interfaces and Classes: The IEvent marker interface distinguishes events, while OrderPlacedEvent, InventoryReservedEvent, and PaymentCompletedEvent represent the workflow stages.

* 2.EventBus: Manages publishing and subscribing to events. Services subscribe to relevant events and respond when the event is published.

* 3.OrderSaga: Orchestrates the workflow by listening to and publishing events. It starts the saga, handles each event, and coordinates state transitions.
 
 */