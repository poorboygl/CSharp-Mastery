using System;
using System.ComponentModel.DataAnnotations;

public class Program
{
    static void Main()
    {
        var calculator = new FibonacciCalculator();

        Console.WriteLine("=== Fibonacci Calculator ===");
        Console.Write("Enter n: ");

        if (int.TryParse(Console.ReadLine(), out int n))
        {
            long result = calculator.Calculate(n);
            Console.WriteLine($"Result: Fibonacci({n}) = {result}");
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}


public class FibonacciCalculator
{
    private readonly Dictionary<int, long> _memo = new()
        {
            { 0, 0 },
            { 1, 1 }
        };

    public long Calculate(int n)
    {
        if (_memo.ContainsKey(n))
        {
            return _memo[n];
        }

        long result = Calculate(n - 1) + Calculate(n - 2);
        _memo[n] = result; // Store result in memo dictionary
        return result;
    }
}

/*
* 1.Memoization Dictionary:

    The _memo dictionary stores previously calculated Fibonacci values to avoid redundant recursive calls, improving efficiency.

    Base cases F(0) = 0 and F(1) = 1 are pre-seeded in _memo.

* 2.Recursive Calculation with Memoization:

    Calculate first checks if n is already in _memo and returns it if so.

    Otherwise, it recursively calculates Calculate(n - 1) + Calculate(n - 2), stores the result in _memo, and returns it.

This approach significantly reduces the number of recursive calls compared to a naive recursive implementation.
 */
