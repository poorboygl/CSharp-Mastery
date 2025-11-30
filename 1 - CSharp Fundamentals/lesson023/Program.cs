public class Program
{
    static void Main()
    {
        var calculator = new ArrayCalculator();

        int[] numbers = { 2, 3, 5, 8, 11 };

        Console.WriteLine("=== ProductOfOddNumbers Result ===");
        Console.WriteLine("Input Array: " + string.Join(", ", numbers));

        int result = calculator.ProductOfOddNumbers(numbers);

        Console.WriteLine("Product of Odd Numbers: " + result);

        Console.ReadKey();
    }
}

public class ArrayCalculator
{
    public int ProductOfOddNumbers(int[] numbers)
    {
        int product = 1;
        bool hasOdd = false;

        foreach (var number in numbers)
        {
            if (number % 2 != 0)
            {
                product *= number;
                hasOdd = true;
            }
        }

        return hasOdd ? product : 1;
    }
}

/*
* 1.Initialize Product and Check for Odd Numbers:

product starts at 1, and hasOdd is a boolean flag to check if any odd numbers are found.

* 2.Conditional Multiplication:

For each number in numbers, if it’s odd (number % 2 != 0), multiply it with product and set hasOdd to true.

* 3.Return Product or 1:

If no odd numbers were found (hasOdd remains false), return 1. Otherwise, return the product.
 
*/
