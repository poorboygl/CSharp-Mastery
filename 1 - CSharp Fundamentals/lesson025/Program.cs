public class Program
{
    static void Main()
    {
        var counter = new WordCounter();

        string sentence = "hello world hello chatgpt world hello";

        Console.WriteLine("=== CountWordOccurrences Result ===");
        Console.WriteLine("Input Sentence: " + sentence);

        var result = counter.CountWordOccurrences(sentence);

        Console.WriteLine("Word Counts:");
        foreach (var pair in result)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

        Console.ReadKey();
    }
}

public class WordCounter
{
    public Dictionary<string, int> CountWordOccurrences(string sentence)
    {
        var wordCounts = new Dictionary<string, int>();
        var words = sentence.Split(' ');

        foreach (var word in words)
        {
            if (string.IsNullOrWhiteSpace(word)) continue;

            if (wordCounts.ContainsKey(word))
            {
                wordCounts[word]++;
            }
            else
            {
                wordCounts[word] = 1;
            }
        }

        return wordCounts;
    }
}

/*
* 1.Split Sentence into Words:

sentence.Split(' ') creates an array of words.

* 2.Loop Through Words:

For each word, check if it’s already in wordCounts.

If it is, increment the count; if not, add it with a count of 1.

* 3.Handle Empty Words:

Skip any empty or whitespace words using string.IsNullOrWhiteSpace(word).
 
 */