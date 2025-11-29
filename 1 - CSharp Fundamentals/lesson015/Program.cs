public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        Console.WriteLine("=== SECOND LARGEST NUMBER FINDER ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 5, 3, 8, 8, 2, 10):");
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

                int secondLargest = analyzer.FindSecondLargest(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine($"The second largest distinct number is: {secondLargest}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
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
    public int FindSecondLargest(int[] numbers)
    {
        if (numbers.Length < 2)
        {
            throw new ArgumentException("Array must contain at least two distinct numbers.");
        }

        int? firstLargest = null;
        int? secondLargest = null;

        foreach (var number in numbers)
        {
            if (number == firstLargest || number == secondLargest) continue;

            if (firstLargest == null || number > firstLargest)
            {
                secondLargest = firstLargest;
                firstLargest = number;
            }
            else if (secondLargest == null || number > secondLargest)
            {
                secondLargest = number;
            }
        }

        if (secondLargest == null)
        {
            throw new ArgumentException("Array must contain at least two distinct numbers.");
        }

        return secondLargest.Value;
    }
}

/*
!This exercise helps beginners learn about finding specific values in arrays and handling edge cases.

* 1.Track Two Largest Values:

Use firstLargest and secondLargest to track the largest and second largest numbers, respectively.

* 2.Distinct Check:

Update secondLargest only if the number is distinct from firstLargest.

* 3.Handle Edge Cases:

If there aren’t enough distinct values, throw an ArgumentException.
 
 */