public class Program
{
    static void Main()
    {
        var analyzer = new StringAnalyzer();

        string input = "A man, a plan, a canal, Panama";

        Console.WriteLine("=== IsPalindromeSentence Result ===");
        Console.WriteLine("Input Sentence: " + input);

        bool result = analyzer.IsPalindromeSentence(input);

        Console.WriteLine("Is Palindrome: " + result);

        Console.ReadKey();
    }
}
public class StringAnalyzer
{
    public bool IsPalindromeSentence(string sentence)
    {
        var cleaned = new string(sentence.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        int length = cleaned.Length;

        for (int i = 0; i < length / 2; i++)
        {
            if (cleaned[i] != cleaned[length - i - 1])
            {
                return false;
            }
        }
        return true;
    }
}

/*
* 1.Filter and Normalize:

sentence.Where(char.IsLetterOrDigit).ToArray() removes non-alphanumeric characters.

ToLower() makes the comparison case-insensitive.

* 2.Palindrome Check:

A loop compares each character from the start and end moving towards the center. If any characters don’t match, it returns false.

* 3.Return True if Palindrome:

If the loop completes without finding mismatches, the sentence is a palindrome.
 */