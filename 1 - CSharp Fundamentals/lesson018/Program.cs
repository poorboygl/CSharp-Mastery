public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        Console.WriteLine("=== EVEN NUMBER COUNTER ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 2, 5, 8, 11, 14):");
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

                int count = analyzer.CountEvenNumbers(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine($"The array contains {count} even number(s).");
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

public class ArrayAnalyzer
{
    public int CountEvenNumbers(int[] numbers)
    {
        int count = 0;
        foreach (var number in numbers)
        {
            if (number % 2 == 0)
            {
                count++;
            }
        }
        return count;
    }
}

/*
* 1.Initialize Counter:

count is initialized to 0 to keep track of the number of even numbers.

* 2.Conditional Check:

For each number in numbers, number % 2 == 0 checks if it is even, and count is incremented if it is.

* 3.Return Total Count:

After the loop, count holds the total number of even numbers, which is then returned.
 
 */