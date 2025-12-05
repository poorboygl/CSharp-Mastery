public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Event & Subscriber Pattern Demo ===\n");

        // Create events
        var sportsEvent = new SportsEvent();
        var weatherEvent = new WeatherEvent();

        // Create subscribers
        var userA = new MobileSubscriber("Alice");
        var userB = new MobileSubscriber("Bob");
        var emailSubscriber = new EmailSubscriber();

        // Subscribe
        sportsEvent.Subscribe(userA);
        sportsEvent.Subscribe(emailSubscriber);

        weatherEvent.Subscribe(userB);
        weatherEvent.Subscribe(emailSubscriber);

        // Trigger events
        Console.WriteLine("Triggering Sports Event...");
        sportsEvent.TriggerScoreUpdate("Team A scored a goal!");

        Console.WriteLine("\nTriggering Weather Event...");
        weatherEvent.TriggerWeatherUpdate("Heavy rain expected tonight.");

        // Unsubscribe demo
        Console.WriteLine("\nBob unsubscribes from Weather Alerts.\n");
        weatherEvent.Unsubscribe(userB);

        Console.WriteLine("Triggering Weather Event again...");
        weatherEvent.TriggerWeatherUpdate("Temperature dropping to 15°C.");

        Console.ReadKey();
    }
}

public interface ISubscriber
{
    void Update(string message);
}

public abstract class Event
{
    private readonly List<ISubscriber> _subscribers = new List<ISubscriber>();

    public void Subscribe(ISubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    protected void NotifySubscribers(string message)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Update(message);
        }
    }
}

public class SportsEvent : Event
{
    public void TriggerScoreUpdate(string scoreUpdate)
    {
        NotifySubscribers($"Sports Update: {scoreUpdate}");
    }
}

public class WeatherEvent : Event
{
    public void TriggerWeatherUpdate(string weatherUpdate)
    {
        NotifySubscribers($"Weather Alert: {weatherUpdate}");
    }
}

public class MobileSubscriber : ISubscriber
{
    public string Name { get; }

    public MobileSubscriber(string name)
    {
        Name = name;
    }

    public void Update(string message)
    {
        Console.WriteLine($"{Name} received: {message}");
    }
}

public class EmailSubscriber : ISubscriber
{
    public void Update(string message)
    {
        Console.WriteLine($"Email Notification: {message}");
    }
}

/*
* 1.ISubscriber Interface:

    Provides a contract for any class that will receive notifications.

* 2.Event Class:

    Manages a list of ISubscriber instances.

    Subscribe and Unsubscribe add and remove subscribers, while NotifySubscribers iterates over them, calling Update.

* 3.SportsEvent and WeatherEvent:

    Use NotifySubscribers to broadcast specific updates, passing custom messages to subscribers.

* 4.Subscriber Classes:

    MobileSubscriber takes a Name and displays the message with the subscriber’s name.

    EmailSubscriber displays the message in a format for email notifications.
 */