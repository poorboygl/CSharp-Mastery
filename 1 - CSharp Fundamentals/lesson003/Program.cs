public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Even or Odd Number Checker ===\n");

        Console.Write("Enter an integer: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number))
        {
            NumberChecker checker = new NumberChecker();
            bool isEven = checker.IsEven(number);

            if (isEven)
                Console.WriteLine($"\nThe number {number} is EVEN.");
            else
                Console.WriteLine($"\nThe number {number} is ODD.");
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter a valid integer.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
public class NumberChecker
{
    public bool IsEven(int number)
    {
        return number % 2 == 0;
    }
}

/*
 !This exercise helps to understand conditional statements and how to use the modulus operator.

* 1.Checking for Even or Odd:

Using the modulus operator % to check if number % 2 == 0. If true, the number is even; otherwise, it’s odd.

* 2.Returning the Result:

The method should return true for even numbers and false for odd numbers.
 
 */