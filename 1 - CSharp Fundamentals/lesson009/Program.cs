public class Program
{
    static void Main()
    {
        var checker = new PalindromeChecker();

        Console.WriteLine("=== PALINDROME CHECKER ===");
        Console.Write("Enter a text to check: ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            bool isPalindrome = checker.IsPalindrome(input);

            Console.WriteLine("\n=== RESULT ===");
            if (isPalindrome)
            {
                Console.WriteLine($"\"{input}\" is a palindrome.");
            }
            else
            {
                Console.WriteLine($"\"{input}\" is NOT a palindrome.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input!");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class PalindromeChecker
{
    public bool IsPalindrome(string text)
    {
        string cleanedText = new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        int length = cleanedText.Length;

        for (int i = 0; i < length / 2; i++)
        {
            if (cleanedText[i] != cleanedText[length - i - 1])
            {
                return false;
            }
        }
        return true;
    }
}

/*
 !Explanation of the Solution

    * 1.Normalization:

    Where(char.IsLetterOrDigit) filters out non-alphanumeric characters, and ToLower() converts the text to lowercase.

    * 2.Loop and Comparison:

    The loop iterates to the middle of the string, checking if characters at symmetric positions match.

    If a mismatch is found, it returns false; otherwise, it returns true
 
 */