public class Program
{
    static void Main()
    {
        var manipulator = new ArrayManipulator();

        int[] numbers = { 7, 2, 9, 1, 5 };

        Console.WriteLine("=== SortAndReverseArray Result ===");
        Console.WriteLine("Input Array: " + string.Join(", ", numbers));

        int[] result = manipulator.SortAndReverseArray(numbers);

        Console.WriteLine("Sorted Descending Array: " + string.Join(", ", result));

        Console.ReadKey();
    }
}


public class ArrayManipulator
{
    public int[] SortAndReverseArray(int[] numbers)
    {
        Array.Sort(numbers);      // Sorts in ascending order
        Array.Reverse(numbers);   // Reverses to get descending order
        return numbers;
    }
}


/*
* 1.Sorting in Ascending Order:

Array.Sort(numbers) sorts the numbers array in ascending order.

* 2.Reversing to Get Descending Order:

Array.Reverse(numbers) reverses the sorted array, changing the order to descending.

* 3.Return Modified Array:

The modified array is returned, now in descending order
 
 */