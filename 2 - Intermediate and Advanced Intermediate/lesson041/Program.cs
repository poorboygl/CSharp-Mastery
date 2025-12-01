public class Program
{
    static async Task Main()
    {
        var simulator = new TaskSimulator();
        int delay = 3;

        Console.WriteLine("=== TaskSimulator Result ===");
        Console.WriteLine($"Simulating task with {delay} seconds delay...");

        await simulator.SimulateTaskWithDelay(delay);

        Console.WriteLine("Simulation Finished");

        Console.ReadKey();
    }
}

public class TaskSimulator
{
    public async Task SimulateTaskWithDelay(int delayInSeconds)
    {
        int delayInMilliseconds = delayInSeconds * 1000;
        await Task.Delay(delayInMilliseconds);
        Console.WriteLine("Task Completed");
    }
}

/*
* 1.Convert Delay to Milliseconds:

delayInMilliseconds is calculated by multiplying delayInSeconds by 1000.

* 2.Await Asynchronous Delay:

await Task.Delay(delayInMilliseconds) asynchronously waits for the specified delay.

* 3.Print Completion Message:

After the delay, "Task Completed" is printed to indicate that the task has finished.
 */
