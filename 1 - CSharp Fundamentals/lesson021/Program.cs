public class Program
{
    static void Main()
    {
        var analyzer = new ArrayAnalyzer();

        int[] numbers = { 1, 4, 7, 10, 13, 16 };

        Console.WriteLine("=== CalculateOddAverage Result ===");
        Console.WriteLine("Input Array: " + string.Join(", ", numbers));

        double avg = analyzer.CalculateOddAverage(numbers);

        Console.WriteLine("Average of Odd Numbers: " + avg);

        Console.ReadKey();
    }
}

public class ArrayAnalyzer
{
    public double CalculateOddAverage(int[] numbers)
    {
        int sum = 0;
        int count = 0;

        foreach (var number in numbers)
        {
            if (number % 2 != 0)
            {
                sum += number;
                count++;
            }
        }

        return count > 0 ? (double)sum / count : 0.0;
    }
}

/*
* 1.Sum and Count Odd Numbers:

sum keeps track of the total of odd numbers, and count tracks how many odd numbers were found.

* 2.Calculate Average:

If count is greater than 0, (double)sum / count calculates the average. Otherwise, 0.0 is returned.

* 3.Return Type Casting:

The division sum / count is cast to double to ensure the result includes decimal precision.
 */