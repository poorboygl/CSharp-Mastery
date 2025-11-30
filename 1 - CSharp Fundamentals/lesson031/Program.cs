public class Program
{
    static void Main()
    {
        var analyzer = new NumberAnalyzer();

        int number = 29;

        Console.WriteLine("=== IsPrime Result ===");
        Console.WriteLine("Input Number: " + number);

        bool isPrime = analyzer.IsPrime(number);

        Console.WriteLine("Is Prime: " + isPrime);

        Console.ReadKey();
    }
}

public class NumberAnalyzer
{
    public bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}

/*
* 1.Handle Non-Prime Cases:

If number is less than 2, it is not prime.

* 2.Loop for Divisors:

Use a for loop to iterate from 2 up to Math.Sqrt(number). If number is divisible by i, then it’s not prime.

* 3.Return True if Prime:

If no divisors are found in the loop, number is prime.
 
 */