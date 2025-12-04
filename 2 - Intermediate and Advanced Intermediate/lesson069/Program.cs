public class Program
{
    static void Main()
    {
        var bit = new BitManipulator();

        Console.WriteLine("=== Bit Manipulation Demo ===");

        Console.Write("Enter a number: ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine($"\nIsPowerOfTwo({n}) = {bit.IsPowerOfTwo(n)}");
            Console.WriteLine($"CountSetBits({n}) = {bit.CountSetBits(n)}");
            Console.WriteLine($"FlipBits({n}) = {bit.FlipBits(n)}");
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
public class BitManipulator
{
    public bool IsPowerOfTwo(int n)
    {
        // Check if n is positive and has only one '1' bit
        return n > 0 && (n & (n - 1)) == 0;
    }

    public int CountSetBits(int n)
    {
        int count = 0;
        while (n != 0)
        {
            n &= (n - 1); // Remove the lowest set bit
            count++;
        }
        return count;
    }

    public int FlipBits(int n)
    {
        return ~n;
    }
}

/*
 * 1.IsPowerOfTwo:

    A number is a power of two if it has only one 1 bit in its binary representation.

    (n & (n - 1)) == 0 clears the lowest set bit, and it will be zero if n is a power of two.

* 2.CountSetBits:

    The operation n &= (n - 1) removes the lowest 1 bit in n.

    This is repeated until n becomes 0, and we count each iteration.

* 3.FlipBits:

    ~n inverts all bits in n by flipping each 0 to 1 and each 1 to 0.
 */