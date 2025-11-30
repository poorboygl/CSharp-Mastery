public class Program
{
    static void Main()
    {
        var analyzer = new ListAnalyzer();

        var list1 = new List<int> { 1, 2, 3, 4, 5 };
        var list2 = new List<int> { 3, 5, 7, 9 };

        Console.WriteLine("=== FindCommonElements Result ===");
        Console.WriteLine("List 1: " + string.Join(", ", list1));
        Console.WriteLine("List 2: " + string.Join(", ", list2));

        var common = analyzer.FindCommonElements(list1, list2);

        Console.WriteLine("Common Elements: " + string.Join(", ", common));

        Console.ReadKey();
    }
}

public class ListAnalyzer
{
    public List<int> FindCommonElements(List<int> list1, List<int> list2)
    {
        var set = new HashSet<int>(list1);
        var commonElements = new List<int>();

        foreach (var number in list2)
        {
            if (set.Contains(number))
            {
                commonElements.Add(number);
            }
        }

        return commonElements;
    }
}

/*
* 1.Initialize HashSet:

set is created from list1, allowing fast lookups.

* 2.Loop Through list2:

For each number in list2, check if set contains it. If true, add number to commonElements.

* 3.Return Common Elements:

commonElements contains all elements found in both lists.
 */

