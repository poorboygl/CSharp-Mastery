public class Program
{
    static void Main()
    {
        var analyzer = new NestedListAnalyzer();

        var nestedList = new List<List<int>>
        {
            new() { 3, 5, 1 },
            new() { 10, 2 },
            new() { 7, 8, 4 }
        };

        Console.WriteLine("=== FindMaxInNestedList Result ===");
        Console.WriteLine("Input Nested List:");
        for (int i = 0; i < nestedList.Count; i++)
        {
            Console.WriteLine($"Sublist {i + 1}: {string.Join(", ", nestedList[i])}");
        }

        int maxValue = analyzer.FindMaxInNestedList(nestedList);

        Console.WriteLine("Maximum Value in Nested List: " + maxValue);

        Console.ReadKey();
    }
}

public class NestedListAnalyzer
{
    public int FindMaxInNestedList(List<List<int>> nestedList)
    {
        return nestedList.SelectMany(sublist => sublist).DefaultIfEmpty(int.MinValue).Max();
    }
}

/*
* 1.Flatten Nested List:

nestedList.SelectMany(sublist => sublist) creates a single sequence from the nested list, containing all integers.

* 2.Handle Empty Sequence:

DefaultIfEmpty(int.MinValue) provides a fallback value if the flattened list is empty.

* 3.Find Maximum Value:

Max() finds the highest integer in the sequence, or returns int.MinValue if the sequence is empty.
 */
