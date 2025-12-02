public class Program
{
    static void Main()
    {
        // Tạo WeatherStation
        var station = new WeatherStation();

        // Tạo các observer
        var display = new DisplayDevice();
        var logger = new LoggerDevice();

        // Đăng ký observer
        station.RegisterObserver(display);
        station.RegisterObserver(logger);

        // Mô phỏng cập nhật nhiệt độ
        station.UpdateTemperature(25.5);
        station.UpdateTemperature(26.2);
        station.UpdateTemperature(27.8);

        // In log
        Console.WriteLine("\n--- Logger Data ---");
        foreach (var temp in logger.Log)
        {
            Console.WriteLine($"Logged: {temp}°C");
        }

        Console.ReadKey();
    }
}

public interface IObserver
{
    void Update(double temperature);
}

public class WeatherStation
{
    public double Temperature { get; private set; }
    private List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void DeregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void UpdateTemperature(double newTemperature)
    {
        if (newTemperature != Temperature)
        {
            Temperature = newTemperature;
            NotifyObservers();
        }
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(Temperature);
        }
    }
}

public class DisplayDevice : IObserver
{
    public void Update(double temperature)
    {
        Console.WriteLine($"Current temperature: {temperature}°C");
    }
}

public class LoggerDevice : IObserver
{
    public List<double> Log { get; private set; } = new List<double>();

    public void Update(double temperature)
    {
        Log.Add(temperature);
    }
}

/*
Current temperature: 25.5°C
Current temperature: 26.2°C
Current temperature: 27.8°C

--- Logger Data ---
Logged: 25.5°C
Logged: 26.2°C
Logged: 27.8°C
 */


/*
* 1.Observer Management:

    WeatherStation includes methods to register and deregister observers.

* 2.Notify Observers:

    NotifyObservers iterates over each observer, calling their Update method with the latest temperature.

* 3.Observer Implementations:

    DisplayDevice shows the temperature.

    LoggerDevice logs each temperature change in a list.
 
 */