public class Program
{
    static void Main()
    {
        var reverser = new StringReverser();

        string input = "Hello World!";

        Console.WriteLine("=== StringReverser Result ===");
        Console.WriteLine("Original String: " + input);

        string reversed = reverser.ReverseString(input);

        Console.WriteLine("Reversed String: " + reversed);

        Console.ReadKey();
    }
}


public class StringReverser
{
    public string ReverseString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text; // Handle null or empty strings
        }

        Stack<char> stack = new Stack<char>();

        // Push each character onto the stack
        foreach (char c in text)
        {
            stack.Push(c);
        }

        // Pop characters from the stack to reverse the string
        char[] reversedChars = new char[text.Length];
        int index = 0;

        while (stack.Count > 0)
        {
            reversedChars[index++] = stack.Pop();
        }

        return new string(reversedChars);
    }
}

/*
* 1.ReverseString Method:

    Uses a Stack<char> to store each character of the string.

    Pops characters off the stack to build the reversed string.
 */