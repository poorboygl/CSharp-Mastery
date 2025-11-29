public class Program
{
    static void Main()
    {
        Console.WriteLine("=== STRING REVERSER ===\n");
        var manipulator = new StringManipulator();

        Console.Write("Enter a string: ");
        string input = Console.ReadLine();

        string reversed = manipulator.ReverseString(input);

        Console.WriteLine("Reversed string: " + reversed);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class StringManipulator
{
    public string ReverseString(string text)
    {
        char[] charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}

/*
 !Explanation of the Solution

    * 1.Convert to char Array:

    ToCharArray() converts the string to a character array, which allows easy manipulation.

    * 2.Reverse the Array:

    Array.Reverse(charArray) reverses the characters in the array in place.

    * 3.Return the Reversed String:

    new string(charArray) creates a new string from the reversed character array.
 */