public class Program
{
    static void Main()
    {
        var repo = new FactorialRepository();
        
        Console.WriteLine("=== FACTORIAL CALCULATOR ===");
        Console.Write("Enter a number to calculate its factorial: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number))
        {
            try
            {
                int result = repo.CalculateFactorial(number);
                Console.WriteLine($"The factorial of {number} is: {result}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
public class FactorialRepository
{
    public int CalculateFactorial(int number)
    {
        if (number < 0)
        {
            throw new ArgumentException("Number must be non-negative");
        }

        int result = 1;
        for (int i = 1; i <= number; i++)
        {
            result *= i;
        }
        return result;
    }

    public int CalculateFactorial_Recursive(int number)
    {
        if (number < 0)
        {
            throw new ArgumentException("Number must be non-negative");
        }
        if (number == 0 || number == 1)
        {
            return 1;
        }
        return number * CalculateFactorial_Recursive(number - 1);
    }
}

/*
 * 1.Loop Method:

Initializes result to 1 and multiplies it by each integer up to number.

* 2.Recursive Method:

Base cases: If number is 0 or 1, it returns 1.

For other cases, it calls CalculateFactorial(number - 1) and multiplies by number.

 */