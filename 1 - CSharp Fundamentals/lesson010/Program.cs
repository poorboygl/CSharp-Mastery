public class Program
{
    static void Main()
    {
        var analyzer = new SentenceAnalyzer();

        Console.WriteLine("=== SENTENCE WORD COUNTER ===");
        Console.Write("Enter a sentence: ");

        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            int wordCount = analyzer.CountWords(input);

            Console.WriteLine("\n=== RESULT ===");
            Console.WriteLine($"The sentence contains {wordCount} word(s).");
        }
        else
        {
            Console.WriteLine("\nInvalid input! Sentence cannot be empty.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class SentenceAnalyzer
{
    public int CountWords(string sentence)
    {
        var words = sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
}

/*
!Explanation of the Solution

* 1.Splitting the String:

sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) splits the string by spaces and removes any empty entries that result from multiple spaces.

* 2.Returning Word Count:

words.Length returns the count of words in the sentence.
 */