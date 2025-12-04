public class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== Rate Limited Task Processor Demo ===");

        var processor = new RateLimitedProcessor();

        // Enqueue 5 sample tasks
        for (int i = 1; i <= 5; i++)
        {
            int taskId = i;
            processor.Enqueue(async () =>
            {
                Console.WriteLine($"Task {taskId} executed at {DateTime.Now:HH:mm:ss.fff}");
                await Task.CompletedTask;
            });
        }

        Console.WriteLine("Starting processing (2 tasks per second)...");
        using var cts = new CancellationTokenSource();

        await processor.StartProcessing(tasksPerSecond: 2, cancellationToken: cts.Token);

        Console.WriteLine("Processing completed.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}


public class RateLimitedProcessor
{
    private readonly Queue<Func<Task>> _taskQueue = new();

    public void Enqueue(Func<Task> taskFunc)
    {
        _taskQueue.Enqueue(taskFunc);
    }

    public async Task StartProcessing(int tasksPerSecond, CancellationToken cancellationToken)
    {
        int delay = 1000 / tasksPerSecond;

        while (_taskQueue.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            var taskFunc = _taskQueue.Dequeue();
            await taskFunc();
            await Task.Delay(delay, cancellationToken);
        }
    }
}

/*
* 1.Task Queue:

    _taskQueue holds tasks to be processed.

    Enqueue adds tasks to this queue.

* 2.Controlled Processing in StartProcessing:

    StartProcessing calculates the delay based on tasksPerSecond.

    Each task is dequeued, executed, and followed by a Task.Delay to control the rate.

    The cancellationToken stops processing if triggered.
 */
