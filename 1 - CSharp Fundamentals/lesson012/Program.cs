public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        Console.WriteLine("=== MIN & MAX VALUE FINDER ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 3, 7, 1, 10):");
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

                var (min, max) = analyzer.FindMinMax(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine($"Minimum value: {min}");
                Console.WriteLine($"Maximum value: {max}");
            }
            catch
            {
                Console.WriteLine("\nInvalid input! Please enter valid numbers separated by commas.");
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


public class ArrayAnalyzer
{
    public (int, int) FindMinMax(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            throw new ArgumentException("Array cannot be empty");
        }

        int minValue = numbers[0];
        int maxValue = numbers[0];

        foreach (var number in numbers)
        {
            if (number < minValue)
            {
                minValue = number;
            }
            if (number > maxValue)
            {
                maxValue = number;
            }
        }

        return (minValue, maxValue);
    }
}

/*
 * 1.Handle Empty Array:

Throws an ArgumentException if the array is empty.

* 2.Initialize and Compare:

minValue and maxValue start as the first element.

For each element, updates minValue if the element is smaller and maxValue if the element is larger.

* 3.Return Tuple:

Returns a tuple containing minValue and maxValue.
 
 */