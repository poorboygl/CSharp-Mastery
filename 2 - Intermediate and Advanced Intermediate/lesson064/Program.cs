public class Program
{
    static void Main()
    {
        Console.WriteLine("===== GENERIC CALCULATOR DEMO =====\n");

        var intCalculator = new Calculator<int>();
        Console.WriteLine("--- Integer Operations ---");
        int a = 10, b = 5;
        Console.WriteLine($"{a} + {b} = {intCalculator.Add(a, b)}");
        Console.WriteLine($"{a} - {b} = {intCalculator.Subtract(a, b)}");
        Console.WriteLine($"{a} * {b} = {intCalculator.Multiply(a, b)}");
        Console.WriteLine($"{a} / {b} = {intCalculator.Divide(a, b)}");

        var doubleCalculator = new Calculator<double>();
        Console.WriteLine("\n--- Double Operations ---");
        double x = 7.5, y = 2.5;
        Console.WriteLine($"{x} + {y} = {doubleCalculator.Add(x, y)}");
        Console.WriteLine($"{x} - {y} = {doubleCalculator.Subtract(x, y)}");
        Console.WriteLine($"{x} * {y} = {doubleCalculator.Multiply(x, y)}");
        Console.WriteLine($"{x} / {y} = {doubleCalculator.Divide(x, y)}");

        Console.WriteLine("\n===== END OF PROGRAM =====");
        Console.ReadKey();
    }
}
public class Calculator<T> where T : struct
{
    public T Add(T a, T b)
    {
        return (dynamic)a + (dynamic)b;
    }

    public T Subtract(T a, T b)
    {
        return (dynamic)a - (dynamic)b;
    }

    public T Multiply(T a, T b)
    {
        return (dynamic)a * (dynamic)b;
    }

    public T Divide(T a, T b)
    {
        if ((dynamic)b == 0)
            throw new DivideByZeroException("Cannot divide by zero.");
        return (dynamic)a / (dynamic)b;
    }
}
/*
 ===== GENERIC CALCULATOR DEMO =====

--- Integer Operations ---
10 + 5 = 15
10 - 5 = 5
10 * 5 = 50
10 / 5 = 2

--- Double Operations ---
7.5 + 2.5 = 10
7.5 - 2.5 = 5
7.5 * 2.5 = 18.75
7.5 / 2.5 = 3

===== END OF PROGRAM =====
 */

/*
* 1.Dynamic Arithmetic:

Each arithmetic method casts the inputs to dynamic, allowing C# to use the standard operators (+, -, *, /) for the provided numeric types.

* 2.Division Handling:

Divide checks if b is zero before attempting division and throws a DivideByZeroException if it is.
 
 */