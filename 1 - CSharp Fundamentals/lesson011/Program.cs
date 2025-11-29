public class Program
{
    static void Main()
    {
        var calculator = new ArrayCalculator();

        Console.WriteLine("=== ARRAY AVERAGE CALCULATOR ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 3, 5, 7, 10):");
        Console.Write("Input: ");

        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            try
            {
                int[] numbers = input
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x.Trim()))
                    .ToArray();

                double average = calculator.CalculateAverage(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine($"The average value is: {average}");
            }
            catch
            {
                Console.WriteLine("\nInvalid input! Please enter only numbers separated by commas.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input! Value cannot be empty.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class ArrayCalculator
{
    public double CalculateAverage(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            return 0.0;
        }

        int sum = 0;
        foreach (var number in numbers)
        {
            sum += number;
        }
        return (double)sum / numbers.Length;
    }
}

/*
 * 1.Handle Empty Array:

Checks if numbers.Length == 0, and if so, returns 0.0.

* 2.Sum and Average Calculation:

sum accumulates the total of all numbers, and (double)sum / numbers.Length calculates the average, casting to double to ensure the result has decimal precision.
 */