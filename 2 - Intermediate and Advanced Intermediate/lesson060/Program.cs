public class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== WEATHER SERVICE FETCH DEMO ===");
        var weatherService = new WeatherService();
        var cities = new List<string> { "Hanoi", "Saigon", "Danang", "Hue", "London" };

        using var cts = new CancellationTokenSource();

        // Cancel sau 300 ms để test logic cancellation
        cts.CancelAfter(300);

        Console.WriteLine("Fetching weather data...\n");

        var results = await weatherService.FetchWeatherForCitiesAsync(cities, cts.Token);

        Console.WriteLine("=== Results (Completed Before Cancellation) ===");
        foreach (var r in results)
        {
            Console.WriteLine(r);
        }

        Console.WriteLine("\nDone.");
        Console.ReadKey();
    }
}

public class WeatherService
{
    private static readonly Random RandomDelay = new();

    public async Task<string> FetchWeatherAsync(string cityName, CancellationToken cancellationToken)
    {
        // Simulate a random delay between 200 and 500 ms
        int delay = RandomDelay.Next(200, 500);
        await Task.Delay(delay, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException(cancellationToken);

        return $"Weather data for {cityName} fetched.";
    }

    public async Task<List<string>> FetchWeatherForCitiesAsync(List<string> cities, CancellationToken cancellationToken)
    {
        var tasks = cities.Select(city => FetchWeatherAsync(city, cancellationToken)).ToList();

        try
        {
            // Await all tasks but handle cancellation gracefully
            var results = await Task.WhenAll(tasks);
            return new List<string>(results);
        }
        catch (OperationCanceledException)
        {
            // Collect only the results from successfully completed tasks
            var completedResults = tasks
                .Where(t => t.IsCompletedSuccessfully)
                .Select(t => t.Result)
                .ToList();

            return completedResults;
        }
    }
}

/*
     === WEATHER SERVICE FETCH DEMO ===
    Fetching weather data...

    === Results (Completed Before Cancellation) ===
    Weather data for Hanoi fetched.
    Weather data for Saigon fetched.
    Weather data for London fetched.

    Done.

*/

/*
* 1.Simulated Asynchronous Weather Fetching:

FetchWeatherAsync simulates an asynchronous delay to represent network latency and respects cancellation if triggered.

* 2.Parallel Fetching with Cancellation Handling:

FetchWeatherForCitiesAsync creates tasks for each city and uses Task.WhenAll to run them concurrently.

Catches OperationCanceledException to gather completed requests if cancellation occurs mid-operation.
 */