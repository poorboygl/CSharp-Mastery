public class Program
{
    static void Main()
    {
        var counter = new WordCounter();

        string sentence = "Hello world hello ChatGPT world HELLO";

        Console.WriteLine("=== CountWordFrequencies Result ===");
        Console.WriteLine("Input Sentence: " + sentence);

        var frequencies = counter.CountWordFrequencies(sentence);

        Console.WriteLine("Word Frequencies:");
        foreach (var pair in frequencies)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

        Console.ReadKey();
    }
}
public class WordCounter
{
    public Dictionary<string, int> CountWordFrequencies(string sentence)
    {
        var wordCounts = new Dictionary<string, int>();
        var words = sentence.Split(' ');

        foreach (var word in words)
        {
            string normalizedWord = word.ToLower();

            if (string.IsNullOrWhiteSpace(normalizedWord))
            {
                continue;
            }

            if (wordCounts.ContainsKey(normalizedWord))
            {
                wordCounts[normalizedWord]++;
            }
            else
            {
                wordCounts[normalizedWord] = 1;
            }
        }

        return wordCounts;
    }
}

/*
* 1.Split and Normalize Words:

sentence.Split(' ') creates an array of words, and each word is converted to lowercase to ensure case insensitivity.

* 2.Track Word Counts in Dictionary:

For each normalizedWord, check if it’s already in wordCounts. If it is, increment its count; otherwise, add it with a count of 1.

* 3.Handle Empty Words:

Use string.IsNullOrWhiteSpace to skip empty entries caused by multiple spaces.
 */