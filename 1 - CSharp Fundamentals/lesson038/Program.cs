public class Program
{
    static void Main()
    {
        var analyzer = new StringAnalyzer();

        string word1 = "Listen";
        string word2 = "Silent";

        Console.WriteLine("=== AreAnagrams Result ===");
        Console.WriteLine($"First Word: {word1}");
        Console.WriteLine($"Second Word: {word2}");

        bool result = analyzer.AreAnagrams(word1, word2);

        Console.WriteLine("Are Anagrams: " + result);

        Console.ReadKey();
    }
}

public class StringAnalyzer
{
    public bool AreAnagrams(string firstWord, string secondWord)
    {
        var normalizedFirst = new string(firstWord.ToLower().Where(char.IsLetterOrDigit).OrderBy(c => c).ToArray());
        var normalizedSecond = new string(secondWord.ToLower().Where(char.IsLetterOrDigit).OrderBy(c => c).ToArray());

        return normalizedFirst == normalizedSecond;
    }
}

/*
    * 1.Normalization:

    ToLower() makes the comparison case-insensitive.

    Where(char.IsLetterOrDigit) removes spaces and any non-alphanumeric characters.

    * 2.Sorting:

    OrderBy(c => c) sorts characters in alphabetical order.

    ToArray() converts the sorted characters to a char array, which is then converted back to a string.

    * 3.Comparison:

    If the sorted versions of both strings are equal, they are anagrams.
 */