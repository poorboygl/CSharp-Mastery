public class Program
{
    static void Main()
    {
        Console.WriteLine("=== StringExtensions Demo ===");

        string text = "hello world from csharp";
        string palindromeText = "A man, a plan, a canal, Panama";

        Console.WriteLine($"Original Text: {text}");
        Console.WriteLine($"Title Case: {text.ToTitleCase()}");

        Console.WriteLine();
        Console.WriteLine($"Palindrome Check Text: {palindromeText}");
        Console.WriteLine($"Is Palindrome: {palindromeText.IsPalindrome()}");

        Console.ReadKey();
    }
}

public static class StringExtensions
{
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }

    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var cleaned = new string(input.ToLower().Where(char.IsLetterOrDigit).ToArray());
        return cleaned.SequenceEqual(cleaned.Reverse());
    }
}

/*
 * 1.ToTitleCase:

    Splits the input into words, capitalizes the first letter of each, and joins the words back into a single string.

* 2.IsPalindrome:

    Cleans the string by converting to lowercase and removing non-alphanumeric characters, then checks if it reads the same backward.
 */