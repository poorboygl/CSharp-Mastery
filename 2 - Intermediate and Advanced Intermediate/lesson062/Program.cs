public class Program
{
    static void Main()
    {
        Console.WriteLine("===== NOTIFICATION SYSTEM DEMO =====\n");

        // Base notifications
        INotification email = new EmailNotification();
        INotification sms = new SmsNotification();
        INotification push = new PushNotification();

        // Decorated notifications
        INotification priorityEmail = new PriorityNotification(email);
        INotification retrySms = new RetryNotification(sms);
        INotification priorityRetryPush = new PriorityNotification(new RetryNotification(push));

        Console.WriteLine("--- Sending Base Notifications ---");
        email.Send("Hello Email!");
        sms.Send("Hello SMS!");
        push.Send("Hello Push!");

        Console.WriteLine("\n--- Sending Decorated Notifications ---");
        priorityEmail.Send("Hello Priority Email!");
        retrySms.Send("Hello Retry SMS!");
        priorityRetryPush.Send("Hello Priority Retry Push!");

        Console.WriteLine("\n===== END OF PROGRAM =====");
        Console.ReadKey();
    }
}
public interface INotification
{
    void Send(string message);
}

public class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Email: {message}");
    }
}

public class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS: {message}");
    }
}

public class PushNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Push: {message}");
    }
}

public abstract class NotificationDecorator : INotification
{
    protected INotification _notification;

    protected NotificationDecorator(INotification notification)
    {
        _notification = notification;
    }

    public virtual void Send(string message)
    {
        _notification.Send(message);
    }
}

public class PriorityNotification : NotificationDecorator
{
    public PriorityNotification(INotification notification) : base(notification) { }

    public override void Send(string message)
    {
        _notification.Send($"Priority! {message}");
    }
}

public class RetryNotification : NotificationDecorator
{
    public RetryNotification(INotification notification) : base(notification) { }

    public override void Send(string message)
    {
        int attempts = 0;
        const int maxRetries = 3;

        while (attempts < maxRetries)
        {
            try
            {
                _notification.Send(message);
                break;
            }
            catch (Exception)
            {
                attempts++;
                if (attempts == maxRetries)
                {
                    Console.WriteLine("Failed to send notification after retries.");
                    throw;
                }
            }
        }
    }
}

/*
 ===== NOTIFICATION SYSTEM DEMO =====

--- Sending Base Notifications ---
Email: Hello Email!
SMS: Hello SMS!
Push: Hello Push!

--- Sending Decorated Notifications ---
Email: Priority! Hello Priority Email!
SMS: Hello Retry SMS!
Push: Priority! Hello Priority Retry Push!

===== END OF PROGRAM =====
 
 */

/*
* 1.Concrete Notifications:

Each notification type implements INotification and simply outputs the message in a format indicating the notification type.

* 2.NotificationDecorator:

Acts as a base class for all decorators, wrapping an INotification and passing Send calls to it.

* 3.PriorityNotification Decorator:

Prefixes the message with "Priority!" before passing it to the wrapped notification.

* 4.RetryNotification Decorator:

Attempts to send the message up to three times, retrying if an exception occurs.
 

 */
