public class Program
{
    static void Main()
    {
        var processor = new ArrayProcessor();

        Console.WriteLine("=== ARRAY DUPLICATE REMOVER ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., 3, 5, 3, 7, 5):");
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

                int[] uniqueNumbers = processor.RemoveDuplicates(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine("Array without duplicates:");
                Console.WriteLine(string.Join(", ", uniqueNumbers));
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

public class ArrayProcessor
{
    public int[] RemoveDuplicates(int[] numbers)
    {
        var uniqueNumbers = new List<int>();
        var seenNumbers = new HashSet<int>();

        foreach (var number in numbers)
        {
            if (!seenNumbers.Contains(number))
            {
                uniqueNumbers.Add(number);
                seenNumbers.Add(number);
            }
        }

        return uniqueNumbers.ToArray();
    }
}

/*
Explanation of the Solution

* 1.Tracking Duplicates:

HashSet<int> seenNumbers stores numbers that have been encountered, automatically ignoring duplicates.

* 2.Adding Unique Elements:

If a number is not in seenNumbers, it is added to both uniqueNumbers (for preserving order) and seenNumbers (to track duplicates).

* 3.Return as Array:

uniqueNumbers.ToArray() converts the list of unique numbers back to an array.

*/