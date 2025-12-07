// ======================================================================
// PROGRAM DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== EVENT SOURCING + CQRS DEMO ===");

        var eventStore = new EventStore();
        var order = new OrderAggregate(eventStore);
        var readModel = new OrderReadModel(eventStore);

        Guid orderId = Guid.NewGuid();

        // Write side - Commands produce events
        order.CreateOrder(orderId);
        order.ShipOrder();
        order.DeliverOrder();

        Console.WriteLine("\n=== READ MODEL QUERY ===");
        Console.WriteLine($"Order Status: {readModel.GetCurrentStatus(orderId)}");

        Console.WriteLine("\n=== ORDER EVENT HISTORY ===");
        foreach (var e in readModel.GetOrderHistory(orderId))
        {
            Console.WriteLine($"- {e.GetType().Name}");
        }

        Console.ReadKey();
    }
}

// ======================================================================
// DOMAIN INTERFACES
// ======================================================================
public interface ICommand { }

public interface IEvent
{
    Guid AggregateId { get; }
}

// ======================================================================
// DOMAIN EVENTS
// ======================================================================
public class OrderCreatedEvent : IEvent
{
    public Guid AggregateId { get; }
    public DateTime CreatedAt { get; }

    public OrderCreatedEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        CreatedAt = DateTime.UtcNow;
    }
}

public class OrderShippedEvent : IEvent
{
    public Guid AggregateId { get; }
    public DateTime ShippedAt { get; }

    public OrderShippedEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        ShippedAt = DateTime.UtcNow;
    }
}

public class OrderDeliveredEvent : IEvent
{
    public Guid AggregateId { get; }
    public DateTime DeliveredAt { get; }

    public OrderDeliveredEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        DeliveredAt = DateTime.UtcNow;
    }
}

// ======================================================================
// EVENT STORE (APPEND-ONLY)
// ======================================================================
public class EventStore
{
    private readonly List<IEvent> _events = new();

    public void SaveEvent(IEvent domainEvent)
    {
        _events.Add(domainEvent);
        Console.WriteLine($"[EventStore] Event saved: {domainEvent.GetType().Name}");
    }

    public List<IEvent> GetEventsForAggregate(Guid aggregateId)
    {
        return _events.FindAll(e => e.AggregateId == aggregateId);
    }
}

// ======================================================================
// AGGREGATE ROOT - REPLAYS EVENTS TO RESTORE STATE
// ======================================================================
public class OrderAggregate
{
    private readonly EventStore _eventStore;

    public Guid OrderId { get; private set; }
    public string? Status { get; private set; } = null;

    public OrderAggregate(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    // COMMAND HANDLERS
    public void CreateOrder(Guid orderId)
    {
        var orderCreated = new OrderCreatedEvent(orderId);
        ApplyEvent(orderCreated);
        _eventStore.SaveEvent(orderCreated);
    }

    public void ShipOrder()
    {
        var orderShipped = new OrderShippedEvent(OrderId);
        ApplyEvent(orderShipped);
        _eventStore.SaveEvent(orderShipped);
    }

    public void DeliverOrder()
    {
        var orderDelivered = new OrderDeliveredEvent(OrderId);
        ApplyEvent(orderDelivered);
        _eventStore.SaveEvent(orderDelivered);
    }

    // ==================================================================
    // EVENT APPLICATION (STATE REBUILDER)
    // ==================================================================
    public void ApplyEvent(IEvent @event)
    {
        switch (@event)
        {
            case OrderCreatedEvent created:
                OrderId = created.AggregateId;
                Status = "Created";
                break;

            case OrderShippedEvent:
                Status = "Shipped";
                break;

            case OrderDeliveredEvent:
                Status = "Delivered";
                break;
        }
    }
}

// ======================================================================
// READ MODEL (SEPARATE QUERY SIDE)
// ======================================================================
public class OrderReadModel
{
    private readonly EventStore _eventStore;

    public OrderReadModel(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public string GetCurrentStatus(Guid orderId)
    {
        var orderAggregate = new OrderAggregate(_eventStore);

        // replay events to restore current state
        foreach (var @event in _eventStore.GetEventsForAggregate(orderId))
        {
            orderAggregate.ApplyEvent(@event);
        }

        return orderAggregate.Status ?? "Unknown";
    }

    public List<IEvent> GetOrderHistory(Guid orderId)
    {
        return _eventStore.GetEventsForAggregate(orderId);
    }
}

/*
! === EVENT SOURCING + CQRS DEMO ===
[EventStore] Event saved: OrderCreatedEvent
[EventStore] Event saved: OrderShippedEvent
[EventStore] Event saved: OrderDeliveredEvent

=== READ MODEL QUERY ===
Order Status: Delivered

=== ORDER EVENT HISTORY ===
- OrderCreatedEvent
- OrderShippedEvent
- OrderDeliveredEvent
 */

/*
* 1.EventStore: Stores events related to each order, supporting retrieval by order ID.

* 2.OrderAggregate: Manages the order’s state and applies each event to track the state over time.

* 3.OrderReadModel: Replays events from EventStore to get the current status or full history of an order.

 */