using System.Text.RegularExpressions;
public class Program
{
    static void Main()
    {
        Console.WriteLine("===== WORD FREQUENCY ANALYZER =====");

        string text = "Hello world! Hello C#. This world is beautiful, and this C# code is awesome.";

        var analyzer = new WordFrequencyAnalyzer();

        Console.WriteLine("\n--- Word Frequency Table ---");
        var frequencies = analyzer.GetWordFrequency(text);
        foreach (var kvp in frequencies.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        Console.WriteLine("\n--- Top 3 Most Frequent Words ---");
        var topWords = analyzer.GetMostFrequentWords(text, 3);
        Console.WriteLine(string.Join(", ", topWords));

        Console.WriteLine("\n===== END =====");

        Console.ReadKey();
    }
}


public class WordFrequencyAnalyzer
{
    public Dictionary<string, int> GetWordFrequency(string text)
    {
        var wordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var words = Regex.Split(text.ToLower(), @"\W+").Where(w => !string.IsNullOrEmpty(w));

        foreach (var word in words)
        {
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

    public List<string> GetMostFrequentWords(string text, int count)
    {
        var wordCounts = GetWordFrequency(text);

        return wordCounts
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key)
            .Take(count)
            .Select(kvp => kvp.Key)
            .ToList();
    }
}


/*
* 1.GetWordFrequency:

Converts the text to lowercase, splits it into words, and counts each word’s occurrences in a dictionary.

* 2.GetMostFrequentWords:

Sorts words by frequency (descending) and alphabetically (ascending), then takes the top count words.
 */