public class Program
{
    static void Main()
    {
        Console.WriteLine("===== FIBONACCI GENERATOR =====");

        var generator = new FibonacciGenerator();

        int n = 10; // bạn có thể đổi số khác

        Console.WriteLine($"\n--- Get Nth Fibonacci (n = {n}) ---");
        try
        {
            int nthValue = generator.GetNthFibonacci(n);
            Console.WriteLine($"Fibonacci number at position {n}: {nthValue}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine($"\n--- Get Fibonacci Sequence (n = {n}) ---");
        try
        {
            var sequence = generator.GetFibonacciSequence(n);
            Console.WriteLine("Sequence: " + string.Join(", ", sequence));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n===== END OF PROGRAM =====");

        Console.ReadKey();
    }
}
public class FibonacciGenerator
{
    public int GetNthFibonacci(int n)
    {
        if (n < 1)
        {
            throw new ArgumentException("n must be greater than 0.");
        }
        if (n == 1) return 0;
        if (n == 2) return 1;

        return GetNthFibonacci(n - 1) + GetNthFibonacci(n - 2);
    }

    public List<int> GetFibonacciSequence(int n)
    {
        if (n < 1)
        {
            throw new ArgumentException("n must be greater than 0.");
        }

        var sequence = new List<int> { 0, 1 };
        for (int i = 2; i < n; i++)
        {
            sequence.Add(sequence[i - 1] + sequence[i - 2]);
        }

        return sequence.GetRange(0, n);
    }
}

/*
 * 1.GetNthFibonacci:

    Uses recursion to calculate the nth Fibonacci number. Base cases handle n = 1 and n = 2.

* 2.GetFibonacciSequence:

    Initializes the list with 0 and 1 and iterates to calculate the rest, making it efficient for larger values of n.
 */