using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== STOCK ML PREDICTION BACKGROUND PROCESSOR ===\n");

        var predictor = new StockPredictionModel();
        var processor = new BackgroundProcessor(predictor);
        var service = new PredictionService(processor);

        using var cts = new CancellationTokenSource();

        // Start background processing
        var processingTask = processor.StartProcessing(cts.Token);

        Console.WriteLine("--- Enqueuing Stock Data ---");

        service.AddStockData(new StockData
        {
            PriceChange = 1.2f,
            Volume = 50000,
            MarketCap = 200_000_000,
            Timestamp = DateTime.Now
        });

        service.AddStockData(new StockData
        {
            PriceChange = -0.4f,
            Volume = 45000,
            MarketCap = 195_000_000,
            Timestamp = DateTime.Now.AddSeconds(1)
        });

        service.AddStockData(new StockData
        {
            PriceChange = 0.7f,
            Volume = 70000,
            MarketCap = 210_000_000,
            Timestamp = DateTime.Now.AddSeconds(2)
        });

        // Wait for predictions to finish
        await Task.Delay(2000);

        Console.WriteLine("\n--- Stopping Background Processor ---");
        cts.Cancel();

        await processingTask;

        Console.WriteLine("\n=== END OF PROGRAM ===");
        Console.ReadKey();
    }
}


// ======================================================================
// DATA MODEL
// ======================================================================
public class StockData
{
    public float PriceChange { get; set; }
    public float Volume { get; set; }
    public float MarketCap { get; set; }
    public DateTime Timestamp { get; set; }
}

// ======================================================================
// INTERFACES
// ======================================================================
public interface IModelPredictor
{
    Task<string> PredictAsync(StockData data);
}

public interface IBackgroundProcessor
{
    void EnqueueData(StockData data);
    Task StartProcessing(CancellationToken cancellationToken);
}

// ======================================================================
// ML PREDICTOR
// ======================================================================
public class StockPredictionModel : IModelPredictor
{
    public async Task<string> PredictAsync(StockData data)
    {
        await Task.Delay(100); // Simulate ML.NET computation

        return data.PriceChange > 0 ? "Rise" : "Fall";
    }
}

// ======================================================================
// BACKGROUND PROCESSOR
// ======================================================================
public class BackgroundProcessor : IBackgroundProcessor
{
    private readonly ConcurrentQueue<StockData> _dataQueue = new();
    private readonly IModelPredictor _predictor;

    public BackgroundProcessor(IModelPredictor predictor)
    {
        _predictor = predictor;
    }

    public void EnqueueData(StockData data)
    {
        _dataQueue.Enqueue(data);
    }

    public async Task StartProcessing(CancellationToken cancellationToken)
    {
        Console.WriteLine("Background processor started...\n");

        while (!cancellationToken.IsCancellationRequested)
        {
            if (_dataQueue.TryDequeue(out var data))
            {
                var prediction = await _predictor.PredictAsync(data);
                Console.WriteLine($"Prediction at {data.Timestamp}: {prediction}");
            }
            else
            {
                await Task.Delay(300);
            }
        }

        Console.WriteLine("Background processor stopped.");
    }
}

// ======================================================================
// SERVICE
// ======================================================================
public class PredictionService
{
    private readonly IBackgroundProcessor _processor;

    public PredictionService(IBackgroundProcessor processor)
    {
        _processor = processor;
    }

    public void AddStockData(StockData data)
    {
        _processor.EnqueueData(data);
        Console.WriteLine($"Data enqueued at {data.Timestamp}");
    }
}

/*
 === STOCK ML PREDICTION BACKGROUND PROCESSOR ===

Background processor started...

--- Enqueuing Stock Data ---
Data enqueued at 12/6/2025 11:07:37 PM
Data enqueued at 12/6/2025 11:07:38 PM
Data enqueued at 12/6/2025 11:07:39 PM
Prediction at 12/6/2025 11:07:37 PM: Rise
Prediction at 12/6/2025 11:07:38 PM: Fall
Prediction at 12/6/2025 11:07:39 PM: Rise

--- Stopping Background Processor ---
Background processor stopped.

=== END OF PROGRAM ===
 
 */

/*
* 1.StockPredictionModel: Simulates a prediction based on the PriceChange feature.

* 2.BackgroundProcessor: Runs a continuous loop to dequeue data points, generate predictions, and output the results.

* 3.PredictionService: Provides an interface for enqueuing new data, feeding it to the background processor.
 */