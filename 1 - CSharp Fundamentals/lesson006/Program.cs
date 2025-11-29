public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Maximum Value Finder ===\n");

        Console.Write("Enter integers separated by commas: ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            string[] parts = input.Split(',');
            int[] numbers = new int[parts.Length];
            bool valid = true;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i].Trim(), out numbers[i]))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                try
                {
                    ArrayAnalyzer analyzer = new ArrayAnalyzer();
                    int max = analyzer.FindMaxValue(numbers);

                    Console.WriteLine($"\nThe maximum value in the array is: {max}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter only integers separated by commas.");
            }
        }
        else
        {
            Console.WriteLine("\nNo input provided.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class ArrayAnalyzer
{
    public int FindMaxValue(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            throw new ArgumentException("Array cannot be empty");
        }

        int maxValue = numbers[0];
        foreach (var number in numbers)
        {
            if (number > maxValue)
            {
                maxValue = number;
            }
        }
        return maxValue;
    }
}

/*
 !Explanation of the Solution

    * 1.Check for Empty Array:

    Throws an exception if the array is empty, as there's no maximum value in an empty set.

    * 2.Initialize and Compare:

    maxValue is initially set to the first element, and each subsequent element is compared to it.

    * 3.Update Max Value:

    If a larger value is found, maxValue is updated. At the end of the loop, it contains the largest number.
 */
