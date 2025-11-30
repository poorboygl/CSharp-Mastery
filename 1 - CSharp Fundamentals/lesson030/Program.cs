public class Program
{
    static void Main()
    {
        var processor = new WordProcessor();

        string sentence = "The quick brown fox jumps over the lazy dog";

        Console.WriteLine("=== FindLongestWord Result ===");
        Console.WriteLine("Input Sentence: " + sentence);

        string longest = processor.FindLongestWord(sentence);

        Console.WriteLine("Longest Word: " + longest);

        Console.ReadKey();
    }
}

public class WordProcessor
{
    public string FindLongestWord(string sentence)
    {
        var words = sentence.Split(' ');
        string longestWord = "";

        foreach (var word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length > longestWord.Length)
            {
                longestWord = word;
            }
        }

        return longestWord;
    }
}

/*
* 1.Split Sentence:

sentence.Split(' ') creates an array where each element is a word from the sentence.

* 2.Loop Through Words:

For each word in words, check if it is not empty (!string.IsNullOrWhiteSpace(word)) and if it is longer than longestWord.

* 3.Return Longest Word:

After the loop, longestWord holds the longest word in the sentence, or it remains an empty string if no words were found.
 */