public class Program
{
    static void Main()
    {
        Console.WriteLine("===== Notification Delegate Demo =====\n");

        var notificationService = new NotificationService();

        var email = new EmailNotifier();
        var sms = new SmsNotifier();

        // Subscribe
        notificationService.Subscribe(email.SendEmailNotification);
        notificationService.Subscribe(sms.SendSmsNotification);

        Console.WriteLine("Sending notifications...");
        notificationService.NotifyAll("Your order has been shipped!");

        // Unsubscribe SMS
        Console.WriteLine("\nUnsubscribing SMS notifier...");
        notificationService.Unsubscribe(sms.SendSmsNotification);

        Console.WriteLine("Sending notifications again...");
        notificationService.NotifyAll("Your package has been delivered!");

        Console.WriteLine("\n===== END =====");
        Console.ReadKey();
    }
}



public delegate void NotificationHandler(string message);

public class NotificationService
{
    public NotificationHandler OnNotification;

    public void Subscribe(NotificationHandler handler)
    {
        OnNotification += handler;
    }

    public void Unsubscribe(NotificationHandler handler)
    {
        OnNotification -= handler;
    }

    public void NotifyAll(string message)
    {
        OnNotification?.Invoke(message);
    }
}

public class EmailNotifier
{
    public void SendEmailNotification(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SmsNotifier
{
    public void SendSmsNotification(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}

/*

* 1.Multicast Delegate:

    OnNotification is a multicast delegate field that holds multiple methods of type NotificationHandler.

    Subscribe adds handlers to OnNotification using +=.

    Unsubscribe removes handlers from OnNotification using -=.

* 2.NotifyAll:

    Invokes all subscribed methods using OnNotification?.Invoke(message), ensuring null safety.
 */