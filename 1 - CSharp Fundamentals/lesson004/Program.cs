public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Array Sum Calculator ===\n");

        Console.Write("Enter integers separated by commas: ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            // Chuyển input thành mảng số nguyên
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
                ArrayCalculator calculator = new ArrayCalculator();
                int sum = calculator.CalculateSum(numbers);
                Console.WriteLine($"\nThe sum of the array is: {sum}");
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

public class ArrayCalculator
{
    public int CalculateSum(int[] numbers)
    {
        int sum = 0;
        foreach (var number in numbers)
        {
            sum += number;
        }
        return sum;
    }
}

/*
!Explanation of the Solution

    * 1.Loop Through Array:

    The foreach loop iterates through each element in numbers, adding each element to sum.

    * 2.Return Statement:

    After the loop completes, sum contains the total of all elements, which is then returned.
 */
