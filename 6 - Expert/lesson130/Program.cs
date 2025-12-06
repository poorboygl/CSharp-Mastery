using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== DEMO EVENT SOURCING + ANALYTICS TRACKING ===\n");

        var eventBus = new EventBus();
        var eventStore = new InMemoryEventStore();
        var analyticsService = new AnalyticsService(eventBus);
        var readModel = new AnalyticsReadModel();

        // Đăng ký lắng nghe event để lưu vào EventStore
        eventBus.Subscribe<ProductViewedEvent>(eventStore.Save);
        eventBus.Subscribe<ProductPurchasedEvent>(eventStore.Save);
        eventBus.Subscribe<ProductAddedToCartEvent>(eventStore.Save);

        // Fake product + user
        Guid productA = Guid.NewGuid();
        Guid userX = Guid.NewGuid();

        Console.WriteLine("--- Simulating User Actions ---");

        analyticsService.TrackProductView(productA);
        analyticsService.TrackProductView(productA);
        analyticsService.TrackProductAdditionToCart(productA, userX);
        analyticsService.TrackProductPurchase(productA, userX);

        Console.WriteLine("\n--- Replaying Events into Read Model ---");

        var allEvents = eventStore.GetAllEvents();
        readModel.ReplayEvents(allEvents);

        Console.WriteLine("\n--- Analytics Result ---");
        Console.WriteLine($"Product {productA} View Count     : {readModel.GetViewCount(productA)}");
        Console.WriteLine($"Product {productA} Purchase Count : {readModel.GetPurchaseCount(productA)}");

        Console.WriteLine("\n=== END OF PROGRAM ===");

        Console.ReadKey();
    }
}

// ======================================================================
// EVENT BUS
// ======================================================================
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
                handler.DynamicInvoke(@event);
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (!_subscriptions.ContainsKey(eventType))
            _subscriptions[eventType] = new List<Delegate>();

        _subscriptions[eventType].Add(handler);
    }
}

// ======================================================================
// EVENT TYPES
// ======================================================================
public class ProductViewedEvent : IEvent
{
    public Guid ProductId { get; }
    public DateTime Timestamp { get; }

    public ProductViewedEvent(Guid productId, DateTime timestamp)
    {
        ProductId = productId;
        Timestamp = timestamp;
    }
}

public class ProductPurchasedEvent : IEvent
{
    public Guid ProductId { get; }
    public Guid UserId { get; }
    public DateTime Timestamp { get; }

    public ProductPurchasedEvent(Guid productId, Guid userId, DateTime timestamp)
    {
        ProductId = productId;
        UserId = userId;
        Timestamp = timestamp;
    }
}

public class ProductAddedToCartEvent : IEvent
{
    public Guid ProductId { get; }
    public Guid UserId { get; }
    public DateTime Timestamp { get; }

    public ProductAddedToCartEvent(Guid productId, Guid userId, DateTime timestamp)
    {
        ProductId = productId;
        UserId = userId;
        Timestamp = timestamp;
    }
}

// ======================================================================
// EVENT STORE
// ======================================================================
public interface IEventStore
{
    void Save(IEvent @event);
    List<IEvent> GetAllEvents();
}

public class InMemoryEventStore : IEventStore
{
    private readonly List<IEvent> _events = new();

    public void Save(IEvent @event) => _events.Add(@event);

    public List<IEvent> GetAllEvents() => new List<IEvent>(_events);
}

// ======================================================================
// ANALYTICS SERVICE
// ======================================================================
public class AnalyticsService
{
    private readonly IEventPublisher _eventPublisher;

    public AnalyticsService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public void TrackProductView(Guid productId)
    {
        _eventPublisher.Publish(new ProductViewedEvent(productId, DateTime.UtcNow));
    }

    public void TrackProductPurchase(Guid productId, Guid userId)
    {
        _eventPublisher.Publish(new ProductPurchasedEvent(productId, userId, DateTime.UtcNow));
    }

    public void TrackProductAdditionToCart(Guid productId, Guid userId)
    {
        _eventPublisher.Publish(new ProductAddedToCartEvent(productId, userId, DateTime.UtcNow));
    }
}

// ======================================================================
// READ MODEL
// ======================================================================
public class AnalyticsReadModel
{
    private readonly Dictionary<Guid, int> _productViewCounts = new();
    private readonly Dictionary<Guid, int> _productPurchaseCounts = new();

    public void ReplayEvents(List<IEvent> events)
    {
        foreach (var e in events)
        {
            switch (e)
            {
                case ProductViewedEvent viewEvent:
                    if (!_productViewCounts.ContainsKey(viewEvent.ProductId))
                        _productViewCounts[viewEvent.ProductId] = 0;
                    _productViewCounts[viewEvent.ProductId]++;
                    break;

                case ProductPurchasedEvent purchaseEvent:
                    if (!_productPurchaseCounts.ContainsKey(purchaseEvent.ProductId))
                        _productPurchaseCounts[purchaseEvent.ProductId] = 0;
                    _productPurchaseCounts[purchaseEvent.ProductId]++;
                    break;
            }
        }
    }

    public int GetViewCount(Guid productId) =>
        _productViewCounts.ContainsKey(productId) ? _productViewCounts[productId] : 0;

    public int GetPurchaseCount(Guid productId) =>
        _productPurchaseCounts.ContainsKey(productId) ? _productPurchaseCounts[productId] : 0;
}

/*
 !=== DEMO EVENT SOURCING + ANALYTICS TRACKING ===

--- Simulating User Actions ---

--- Replaying Events into Read Model ---

--- Analytics Result ---
Product b1d9b562-cd79-4136-bb79-d76f80e6a62b View Count     : 2
Product b1d9b562-cd79-4136-bb79-d76f80e6a62b Purchase Count : 1

=== END OF PROGRAM ===
 */

/*
* 1.EventBus: Manages event subscriptions and publishes events to all registered handlers.

* 2.EventStore: In-memory event store, saving all events and replaying them to build the read model.

* 3.AnalyticsService: Tracks user interactions, such as views, purchases, and cart additions.

* 4.AnalyticsReadModel: Replays events to build and maintain an aggregated state for analytics queries
 */