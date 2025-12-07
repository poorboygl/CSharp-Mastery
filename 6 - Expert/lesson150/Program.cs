// ======================================================================
// PROGRAM ENTRY
// ======================================================================
public class Program
{
    static void Main()
    {
        // Demo CQRS + Event Sourcing

        var eventStore = new EventStore();
        var handler = new OrderCommandHandler(eventStore);
        var projection = new OrderProjection();

        var orderId = Guid.NewGuid();

        // Create Order
        handler.Handle(new CreateOrderCommand(orderId, "Laptop Dell", 1500));

        // Update Order
        handler.Handle(new UpdateOrderCommand(orderId, "Laptop Dell XPS", 1800));

        // Build read model from event history
        projection.Replay(eventStore.GetEvents());

        var order = projection.GetOrder(orderId);

        Console.WriteLine($"ORDER READ MODEL -> {order.ProductName} - {order.Price}");

        Console.ReadKey();
    }
}

// ======================================================================
// COMMANDS
// ======================================================================
public interface ICommand { }

public class CreateOrderCommand : ICommand
{
    public Guid OrderId { get; }
    public string ProductName { get; }
    public decimal Price { get; }

    public CreateOrderCommand(Guid orderId, string productName, decimal price)
    {
        OrderId = orderId;
        ProductName = productName;
        Price = price;
    }
}

public class UpdateOrderCommand : ICommand
{
    public Guid OrderId { get; }
    public string ProductName { get; }
    public decimal Price { get; }

    public UpdateOrderCommand(Guid orderId, string productName, decimal price)
    {
        OrderId = orderId;
        ProductName = productName;
        Price = price;
    }
}

// ======================================================================
// EVENTS
// ======================================================================
public interface IEvent { }

public class OrderCreatedEvent : IEvent
{
    public Guid OrderId { get; }
    public string ProductName { get; }
    public decimal Price { get; }

    public OrderCreatedEvent(Guid orderId, string productName, decimal price)
    {
        OrderId = orderId;
        ProductName = productName;
        Price = price;
    }
}

public class OrderUpdatedEvent : IEvent
{
    public Guid OrderId { get; }
    public string ProductName { get; }
    public decimal Price { get; }

    public OrderUpdatedEvent(Guid orderId, string productName, decimal price)
    {
        OrderId = orderId;
        ProductName = productName;
        Price = price;
    }
}

// ======================================================================
// EVENT STORE (WRITE MODEL)
// ======================================================================
public class EventStore
{
    private readonly List<IEvent> _events = new();

    public void SaveEvent(IEvent @event)
    {
        _events.Add(@event);
    }

    public IEnumerable<IEvent> GetEvents() => _events;
}

// ======================================================================
// COMMAND HANDLER (WRITE-SIDE PROCESSOR)
// ======================================================================
public class OrderCommandHandler
{
    private readonly EventStore _eventStore;

    public OrderCommandHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public void Handle(CreateOrderCommand command)
    {
        var evt = new OrderCreatedEvent(command.OrderId, command.ProductName, command.Price);
        _eventStore.SaveEvent(evt);
    }

    public void Handle(UpdateOrderCommand command)
    {
        var evt = new OrderUpdatedEvent(command.OrderId, command.ProductName, command.Price);
        _eventStore.SaveEvent(evt);
    }
}

// ======================================================================
// PROJECTION (READ MODEL)
// ======================================================================
public class OrderProjection
{
    private readonly Dictionary<Guid, OrderState> _orderStates = new();

    public void Apply(IEvent @event)
    {
        switch (@event)
        {
            case OrderCreatedEvent created:
                _orderStates[created.OrderId] = new OrderState
                {
                    Id = created.OrderId,
                    ProductName = created.ProductName,
                    Price = created.Price
                };
                break;

            case OrderUpdatedEvent updated:
                if (_orderStates.TryGetValue(updated.OrderId, out var order))
                {
                    order.ProductName = updated.ProductName;
                    order.Price = updated.Price;
                }
                break;
        }
    }

    public void Replay(IEnumerable<IEvent> events)
    {
        foreach (var evt in events)
            Apply(evt);
    }

    public OrderState GetOrder(Guid id)
    {
        _orderStates.TryGetValue(id, out var state);
        return state;
    }
}

// ======================================================================
// READ SIDE STATE
// ======================================================================
public class OrderState
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
    public decimal Price { get; set; }
}

/*
* 1.Command and Event Interfaces: ICommand and IEvent marker interfaces distinguish commands from events.

* 2.Commands and Events: CreateOrderCommand and UpdateOrderCommand represent requests, while OrderCreatedEvent and OrderUpdatedEvent capture state changes.

* 3.EventStore: Stores all events, allowing them to be replayed to rebuild the state.

* 4.OrderProjection: The read model that applies events to create a current view of each order.

* 5.OrderCommandHandler: Handles commands and produces the respective events, storing them in the EventStore.
 */