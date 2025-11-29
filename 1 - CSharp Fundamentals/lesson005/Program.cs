public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Vowel Counter ===\n");

        Console.Write("Enter a text: ");
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            StringAnalyzer analyzer = new StringAnalyzer();
            int vowelCount = analyzer.CountVowels(input);

            Console.WriteLine($"\nThe text contains {vowelCount} vowel(s).");
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter a non-empty text.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class StringAnalyzer
{
    public int CountVowels(string text)
    {
        int vowelCount = 0;
        foreach (char c in text.ToLower())
        {
            if ("aeiou".Contains(c))
            {
                vowelCount++;
            }
        }
        return vowelCount;
    }
}

/*
!Explanation of the Solution

    * 1.Loop Through String:

    The foreach loop iterates through each character in text, converting it to lowercase.

    * 2.Vowel Check:

    The condition if ("aeiou".Contains(c)) checks if the character is a vowel.

    * 3.Increment Vowel Count:

    Each time a vowel is found, vowelCount is incremented. After the loop, vowelCount holds the total number of vowels.
 
 */