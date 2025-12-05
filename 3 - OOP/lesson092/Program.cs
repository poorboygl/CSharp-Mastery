public class Program
{
    static void Main()
    {
        Console.WriteLine("=== TEMPERATURE MONITOR DEMO ===\n");

        var monitor = new TemperatureMonitor();
        var alarm = new Alarm();

        // Subscribe to event
        alarm.Subscribe(monitor);

        Console.WriteLine("Setting temperature to 80...");
        monitor.Temperature = 80;   // Không vượt 100 → không báo động

        Console.WriteLine("Setting temperature to 120...");
        monitor.Temperature = 120;  // Vượt 100 → bật báo động

        // Unsubscribe
        alarm.Unsubscribe(monitor);

        Console.WriteLine("\nSetting temperature to 150 after unsubscribing...");
        monitor.Temperature = 150;  // Không báo động nữa

        Console.ReadKey();
    }
}

public class TemperatureMonitor
{
    private double _temperature;

    public delegate void TemperatureExceededEventHandler(object sender, EventArgs e);
    public event TemperatureExceededEventHandler TemperatureExceeded;

    public double Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            if (_temperature > 100)
            {
                OnTemperatureExceeded();
            }
        }
    }

    protected virtual void OnTemperatureExceeded()
    {
        TemperatureExceeded?.Invoke(this, EventArgs.Empty);
    }
}

public class Alarm
{
    public void Alert(object sender, EventArgs e)
    {
        Console.WriteLine("Alert! Temperature has exceeded the safe limit.");
    }

    public void Subscribe(TemperatureMonitor monitor)
    {
        monitor.TemperatureExceeded += Alert;
    }

    public void Unsubscribe(TemperatureMonitor monitor)
    {
        monitor.TemperatureExceeded -= Alert;
    }
}

/*
 === TEMPERATURE MONITOR DEMO ===

Setting temperature to 80...
Setting temperature to 120...
Alert! Temperature has exceeded the safe limit.

 */

/*
* 1.TemperatureMonitor Class:

    TemperatureExceededEventHandler delegate defines the event signature.

    TemperatureExceeded is an event that notifies subscribers when the temperature exceeds the threshold.

    Temperature property checks the temperature, and if it’s above 100, calls OnTemperatureExceeded.

    OnTemperatureExceeded raises the TemperatureExceeded event.

* 2.Alarm Class:

    Alert method is an event handler that prints an alert message.

    Subscribe method allows the Alarm class to subscribe to the TemperatureExceeded event in TemperatureMonitor.

    Unsubscribe method allows the Alarm to stop receiving notifications.
 
 */