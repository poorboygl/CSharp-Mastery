public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        Console.WriteLine("=== POSITIVE NUMBER SUM CALCULATOR ===");
        Console.WriteLine("Enter numbers separated by commas (e.g., -3, 5, 7, -1, 10):");
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

                int sum = analyzer.SumPositiveNumbers(numbers);

                Console.WriteLine("\n=== RESULT ===");
                Console.WriteLine($"The sum of all positive numbers is: {sum}");
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
    public int SumPositiveNumbers(int[] numbers)
    {
        int sum = 0;
        foreach (var number in numbers)
        {
            if (number > 0)
            {
                sum += number;
            }
        }
        return sum;
    }
}

/*
* 1.Initialize Sum:

sum is initialized to 0 to keep track of the total of positive numbers.

* 2.Conditional Check:

For each number in numbers, if number > 0, it is added to sum.

* 3.Return Total Sum:

After the loop, sum contains the sum of all positive numbers, which is then returned.
 */