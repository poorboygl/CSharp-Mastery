public class Program
{
    static void Main()
    {
        var broker = new InMemoryMessageBroker();

        var orderService = new OrderService(broker);
        var inventoryService = new InventoryService(broker);
        var paymentService = new PaymentService(broker);
        var shippingService = new ShippingService(broker);

        broker.Subscribe<OrderShippedEvent>(e =>
        {
            Console.WriteLine($"Order {e.OrderId} shipped successfully!");
        });

        var orderId = Guid.NewGuid();
        orderService.PlaceOrder(orderId);

        Console.ReadKey();
    }
}
public interface ICommand { }
public interface IEvent { }
public interface IMessageBroker
{
    void PublishEvent(IEvent @event);
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}

public class InMemoryMessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public void PublishEvent(IEvent @event)
    {
        var eventType = @event.GetType();
        if (_handlers.ContainsKey(eventType))
        {
            foreach (var handler in _handlers[eventType])
            {
                handler.DynamicInvoke(@event);
            }
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Delegate>();
        }
        _handlers[eventType].Add(handler);
    }
}

public class OrderPlacedEvent : IEvent { public Guid OrderId { get; } public OrderPlacedEvent(Guid orderId) => OrderId = orderId; }
public class InventoryReservedEvent : IEvent { public Guid OrderId { get; } public InventoryReservedEvent(Guid orderId) => OrderId = orderId; }
public class PaymentCompletedEvent : IEvent { public Guid OrderId { get; } public PaymentCompletedEvent(Guid orderId) => OrderId = orderId; }
public class OrderShippedEvent : IEvent { public Guid OrderId { get; } public OrderShippedEvent(Guid orderId) => OrderId = orderId; }
public class InventoryReleasedEvent : IEvent { public Guid OrderId { get; } public InventoryReleasedEvent(Guid orderId) => OrderId = orderId; }

public class OrderService
{
    private readonly IMessageBroker _broker;
    public OrderService(IMessageBroker broker) => _broker = broker;
    public void PlaceOrder(Guid orderId) => _broker.PublishEvent(new OrderPlacedEvent(orderId));
}

public class InventoryService
{
    private readonly IMessageBroker _broker;
    public InventoryService(IMessageBroker broker)
    {
        _broker = broker;
        _broker.Subscribe<OrderPlacedEvent>(HandleOrderPlaced);
    }
    private void HandleOrderPlaced(OrderPlacedEvent @event) => _broker.PublishEvent(new InventoryReservedEvent(@event.OrderId));
}

public class PaymentService
{
    private readonly IMessageBroker _broker;
    public PaymentService(IMessageBroker broker)
    {
        _broker = broker;
        _broker.Subscribe<InventoryReservedEvent>(HandleInventoryReserved);
    }
    private void HandleInventoryReserved(InventoryReservedEvent @event) => _broker.PublishEvent(new PaymentCompletedEvent(@event.OrderId));
}

public class ShippingService
{
    private readonly IMessageBroker _broker;
    public ShippingService(IMessageBroker broker)
    {
        _broker = broker;
        _broker.Subscribe<PaymentCompletedEvent>(HandlePaymentCompleted);
    }
    private void HandlePaymentCompleted(PaymentCompletedEvent @event) => _broker.PublishEvent(new OrderShippedEvent(@event.OrderId));
}

/*
 Event-Driven Architecture (EDA) + Pub/Sub message broker

* 1.InMemoryMessageBroker: Manages event subscriptions and publishes events to the appropriate handlers.

* 2.OrderService, InventoryService, PaymentService, ShippingService: Each service subscribes to the relevant event and triggers the next action in the order workflow.

* 3.Compensation Actions: In a real-world implementation, additional handlers would respond to failed events (e.g., InventoryReleasedEvent) to undo previous actions.
 */