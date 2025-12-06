public class Program
{
    static async Task Main()
    {
        Console.WriteLine("===== ASYNC DATA FETCHER (Timeout + Cancellation) =====\n");

        var fetcher = new DataFetcher();

        Console.WriteLine("Fetching with timeout = 500ms...");
        string result1 = await fetcher.FetchDataWithTimeoutAsync("https://example.com", 500);
        Console.WriteLine($"Result: {result1}\n");

        Console.WriteLine("Fetching with timeout = 1500ms...");
        string result2 = await fetcher.FetchDataWithTimeoutAsync("https://example.com", 1500);
        Console.WriteLine($"Result: {result2}\n");

        // Manual cancellation test
        using var cts = new CancellationTokenSource();
        var task = fetcher.FetchDataAsync("https://google.com", cts.Token);

        Task.Delay(200).ContinueWith(_ => cts.Cancel()); // Cancel after 200ms

        try
        {
            string result3 = await task;
            Console.WriteLine(result3);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Manual cancellation triggered.");
        }

        Console.ReadKey();
    }
}


public class DataFetcher
{
    private static readonly Random RandomDelay = new();

    public async Task<string> FetchDataAsync(string url, CancellationToken cancellationToken)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new ArgumentException("Invalid URL format");
        }

        int delay = RandomDelay.Next(100, 1000);
        await Task.Delay(delay, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(cancellationToken);
        }

        return $"Data fetched from {url}";
    }

    public async Task<string> FetchDataWithTimeoutAsync(string url, int timeoutMs)
    {
        using var timeoutCts = new CancellationTokenSource(timeoutMs);

        try
        {
            return await FetchDataAsync(url, timeoutCts.Token);
        }
        catch (OperationCanceledException)
        {
            return timeoutCts.Token.IsCancellationRequested ? "Timed out" : "Canceled";
        }
        catch (Exception)
        {
            return "Error occurred";
        }
    }
}

/*
* 1.Asynchronous Fetching with Cancellation:

    FetchDataAsync uses Task.Delay to simulate fetching with a delay.

    Throws OperationCanceledException if canceled.

* 2.Timeout and Error Handling:

    FetchDataWithTimeoutAsync creates a CancellationTokenSource with a timeout.

    Uses try-catch to return specific messages for cancellation, timeouts, and other errors.
 */
