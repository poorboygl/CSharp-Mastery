public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        Console.WriteLine("=== NUMBER OCCURRENCE COUNTER ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 5, 3, 8, 3, 2, 3):");
        Console.Write("Array input: ");

        string? arrayInput = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(arrayInput))
        {
            try
            {
                int[] numbers = arrayInput
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x.Trim()))
                    .ToArray();

                Console.Write("Enter the number to count: ");
                string? targetInput = Console.ReadLine();

                if (int.TryParse(targetInput, out int target))
                {
                    int count = analyzer.CountOccurrences(numbers, target);

                    Console.WriteLine("\n=== RESULT ===");
                    Console.WriteLine($"The number {target} appears {count} time(s) in the array.");
                }
                else
                {
                    Console.WriteLine("\nInvalid target number!");
                }
            }
            catch
            {
                Console.WriteLine("\nInvalid array input! Please enter valid numbers separated by commas.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input! Array cannot be empty.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class ArrayAnalyzer
{
    public int CountOccurrences(int[] numbers, int target)
    {
        int count = 0;
        foreach (var number in numbers)
        {
            if (number == target)
            {
                count++;
            }
        }
        return count;
    }
}

/*
* 1.Initialize Counter:

count is initialized to 0 to keep track of the occurrences of target.

* 2.Conditional Check:

For each number in numbers, if number == target, count is incremented.

* 3.Return Count:

After the loop, count represents the total occurrences of target.
 
 */