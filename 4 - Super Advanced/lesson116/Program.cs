public class Program
{
    static void Main()
    {
        var notifier = new Notifier();
        var subscriber = new NotificationSubscriber();

        // Đăng ký lắng nghe sự kiện
        subscriber.Subscribe(notifier);

        // Gửi thông báo
        notifier.SendNotification("System initialized");
        notifier.SendNotification("New order received");
        notifier.SendNotification("Service completed");

        // Hủy đăng ký lắng nghe
        subscriber.Unsubscribe(notifier);

        Console.WriteLine("Finished.");
        Console.ReadKey();
    }
}

public class NotificationEventArgs : EventArgs
{
    public string Message { get; }
    public DateTime TimeStamp { get; }

    public NotificationEventArgs(string message)
    {
        Message = message;
        TimeStamp = DateTime.UtcNow;
    }
}

public class Notifier
{
    public event EventHandler<NotificationEventArgs> NotificationReceived;

    public void SendNotification(string message)
    {
        var args = new NotificationEventArgs(message);
        NotificationReceived?.Invoke(this, args);
    }
}

public class NotificationSubscriber
{
    public void Subscribe(Notifier notifier)
    {
        notifier.NotificationReceived += OnNotificationReceived;
    }

    public void Unsubscribe(Notifier notifier)
    {
        notifier.NotificationReceived -= OnNotificationReceived;
    }

    public void OnNotificationReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"Notification received: {e.Message} at {e.TimeStamp}");
    }
}

/*
* 1.Custom Event Arguments:

    NotificationEventArgs holds the notification message and timestamp, making the event data customizable and extensible.

* 2.Event Handling with NotificationReceived:

    NotificationReceived is triggered in SendNotification, passing along the NotificationEventArgs.

    OnNotificationReceived in NotificationSubscriber handles the event, displaying the notification.
 */
