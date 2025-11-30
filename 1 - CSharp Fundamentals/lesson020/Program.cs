public class Program
{
    static void Main()
    {
        var formatter = new StringFormatter();

        string input = "hello world from c#";

        Console.WriteLine("=== CapitalizeWords Result ===");
        Console.WriteLine("Input Sentence: " + input);

        string result = formatter.CapitalizeWords(input);

        Console.WriteLine("Capitalized Sentence: " + result);

        Console.ReadKey();
    }
}
public class StringFormatter
{
    public string CapitalizeWords(string sentence)
    {
        var words = sentence.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}

/*
* 1.Split the String into Words:

sentence.Split(' ') splits the sentence into an array of words.

* 2.Capitalize Each Word:

The loop iterates over each word. If the word is not empty, char.ToUpper is used to capitalize the first letter, and ToLower converts the rest to lowercase.

* 3.Join the Words:

string.Join(" ", words) combines the words back into a sentence, separated by spaces.
 
 */