public class Program
{
    static void Main()
    {
        var analyzer = new StringAnalyzer();

        Console.WriteLine("=== ANAGRAM CHECKER ===");

        Console.Write("Enter the first word: ");
        string? firstWord = Console.ReadLine();

        Console.Write("Enter the second word: ");
        string? secondWord = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(firstWord) && !string.IsNullOrWhiteSpace(secondWord))
        {
            bool areAnagrams = analyzer.AreAnagrams(firstWord, secondWord);

            Console.WriteLine("\n=== RESULT ===");
            if (areAnagrams)
            {
                Console.WriteLine($"\"{firstWord}\" and \"{secondWord}\" are anagrams.");
            }
            else
            {
                Console.WriteLine($"\"{firstWord}\" and \"{secondWord}\" are NOT anagrams.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input! Words cannot be empty.");
        }

        Console.WriteLine("\nPress any key to exit...");
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