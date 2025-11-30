public class Program
{
    static void Main()
    {
        var processor = new ArrayProcessor();

        int[] numbers = { 5, -3, 10, -7, 0, 8 };

        Console.WriteLine("=== Array Processing Result ===");
        Console.WriteLine("Original Array: " + string.Join(", ", numbers));

        int[] result = processor.ReplaceNegativesWithZero(numbers);

        Console.WriteLine("Modified Array (Negative numbers replaced with 0): " + string.Join(", ", result));

        Console.ReadKey();
    }
}

public class ArrayProcessor
{
    public int[] ReplaceNegativesWithZero(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] < 0)
            {
                numbers[i] = 0;
            }
        }
        return numbers;
    }
}

/*
!This exercise helps beginners understand how to modify elements in an array using loops and conditional checks.

* 1.Loop Through Array:

Use a for loop to check each element by its index.

* 2.Replace Negative Numbers:

If an element is less than 0, set it to 0.
 */