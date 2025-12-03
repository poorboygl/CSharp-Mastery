public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Event Aggregator Demo ===");

        var aggregator = new EventAggregator();

        var emailService = new EmailNotificationService();
        var smsService = new SMSNotificationService();

        // Đăng ký sự kiện
        aggregator.Subscribe<UserRegisteredEvent>(emailService.OnUserRegistered);
        aggregator.Subscribe<OrderPlacedEvent>(smsService.OnOrderPlaced);

        // Kích hoạt sự kiện
        aggregator.Publish(new UserRegisteredEvent());
        aggregator.Publish(new OrderPlacedEvent());

        Console.ReadKey();
    }
}


public interface INotificationEvent
{
    // Marker interface for notification events
}

public class UserRegisteredEvent : INotificationEvent
{
    // Represents a user registration event
}

public class OrderPlacedEvent : INotificationEvent
{
    // Represents an order placed event
}

public class EventAggregator
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> handler) where T : INotificationEvent
    {
        var eventType = typeof(T);
        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = new List<Delegate>();
        }
        _subscribers[eventType].Add(handler);
    }

    public void Publish<T>(T eventItem) where T : INotificationEvent
    {
        var eventType = typeof(T);
        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var handler in _subscribers[eventType])
            {
                (handler as Action<T>)?.Invoke(eventItem);
            }
        }
    }
}

public class EmailNotificationService
{
    public void OnUserRegistered(UserRegisteredEvent userEvent)
    {
        Console.WriteLine("Email sent to new user.");
    }
}

public class SMSNotificationService
{
    public void OnOrderPlaced(OrderPlacedEvent orderEvent)
    {
        Console.WriteLine("SMS sent for new order.");
    }
}

/*
 === Event Aggregator Demo ===
    Email sent to new user.
    SMS sent for new order.

 */

/*
* 1.Event Aggregator:

    Uses a dictionary to store a list of delegates for each event type.

    Subscribe registers event handlers by adding them to the list for each event type.

    Publish triggers each handler associated with the published event type.

* 2.Notification Services:

    EmailNotificationService handles UserRegisteredEvent.

    SMSNotificationService handles OrderPlacedEvent.
 */