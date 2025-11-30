public class Program
{
    static void Main()
    {
        var processor = new ListProcessor();

        var numbers = new List<int> { 1, 2, 3, 2, 4, 1, 5, 3 };

        Console.WriteLine("=== RemoveDuplicates Result ===");
        Console.WriteLine("Input List: " + string.Join(", ", numbers));

        var result = processor.RemoveDuplicates(numbers);

        Console.WriteLine("List Without Duplicates: " + string.Join(", ", result));

        Console.ReadKey();
    }
}
public class ListProcessor
{
    public List<int> RemoveDuplicates(List<int> numbers)
    {
        var uniqueNumbers = new HashSet<int>();

        foreach (var number in numbers)
        {
            uniqueNumbers.Add(number);
        }

        return new List<int>(uniqueNumbers);
    }
}