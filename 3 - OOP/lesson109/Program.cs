public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Weather Station Observer Pattern Demo ===\n");

        var weatherStation = new WeatherStation();

        var currentDisplay = new CurrentConditionsDisplay();
        var statisticsDisplay = new StatisticsDisplay();

        weatherStation.RegisterObserver(currentDisplay);
        weatherStation.RegisterObserver(statisticsDisplay);

        Console.WriteLine("\n[Update 1]");
        weatherStation.SetMeasurements(30.4f, 70f, 1012f);

        Console.WriteLine("\n[Update 2]");
        weatherStation.SetMeasurements(28.9f, 65f, 1010f);

        Console.WriteLine("\n[Update 3]");
        weatherStation.SetMeasurements(32.1f, 80f, 1008f);

        Console.ReadKey();
    }
}


public interface IWeatherObserver
{
    void Update(float temperature, float humidity, float pressure);
}

public class WeatherStation
{
    private readonly List<IWeatherObserver> _observers = new List<IWeatherObserver>();
    private float _temperature;
    private float _humidity;
    private float _pressure;

    public void RegisterObserver(IWeatherObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IWeatherObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_temperature, _humidity, _pressure);
        }
    }

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        _temperature = temperature;
        _humidity = humidity;
        _pressure = pressure;
        NotifyObservers();
    }
}

public class CurrentConditionsDisplay : IWeatherObserver
{
    public void Update(float temperature, float humidity, float pressure)
    {
        Console.WriteLine($"Current conditions: {temperature}C degrees, {humidity}% humidity");
    }
}

public class StatisticsDisplay : IWeatherObserver
{
    private float _maxTemp = float.MinValue;
    private float _minTemp = float.MaxValue;
    private float _tempSum = 0.0f;
    private int _numReadings = 0;

    public void Update(float temperature, float humidity, float pressure)
    {
        _tempSum += temperature;
        _numReadings++;
        _maxTemp = Math.Max(_maxTemp, temperature);
        _minTemp = Math.Min(_minTemp, temperature);
        float avgTemp = _tempSum / _numReadings;

        Console.WriteLine($"Avg/Max/Min temperature = {avgTemp}/{_maxTemp}/{_minTemp}");
    }
}

/*
* 1.IWeatherObserver Interface:

    Defines the Update method, allowing each observer to handle data changes.

* 2.WeatherStation Class with Observer Pattern:

    Stores weather data and maintains a list of observers.

    SetMeasurements updates weather data and calls NotifyObservers, informing all observers of the change.

* 3.Concrete Observers:

    CurrentConditionsDisplay prints the current temperature and humidity.

    StatisticsDisplay tracks the average, maximum, and minimum temperatures across updates.
 */