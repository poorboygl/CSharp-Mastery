public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Generic Sorter Demo ===");

        var people = new List<Person>
        {
            new Person { Name = "Alice", Age = 32 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 29 }
        };

        Console.WriteLine("Before Sorting:");
        foreach (var p in people)
        {
            Console.WriteLine(p);
        }

        var sorter = new Sorter<Person>();
        sorter.Sort(people); // Sort by Age (default CompareTo)

        Console.WriteLine("\nAfter Sorting (by Age):");
        foreach (var p in people)
        {
            Console.WriteLine(p);
        }

        Console.ReadKey();
    }
}

public class Sorter<T> where T : IComparable<T>
{
    public void Sort(List<T> list, IComparer<T> comparer = null)
    {
        comparer ??= Comparer<T>.Default;

        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - 1 - i; j++)
            {
                if (comparer.Compare(list[j], list[j + 1]) > 0)
                {
                    // Swap elements
                    T temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
    }
}

public class Person : IComparable<Person>
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public int CompareTo(Person other)
    {
        if (other == null) return 1;
        return Age.CompareTo(other.Age);
    }

    public override string ToString()
    {
        return $"{Name}, Age: {Age}";
    }
}

/*
 === Generic Sorter Demo ===
Before Sorting:
Alice, Age: 32
Bob, Age: 25
Charlie, Age: 29

After Sorting (by Age):
Bob, Age: 25
Charlie, Age: 29
Alice, Age: 32
 
 */
/*
 * 1.Generic Sorter<T> Class:

    Uses IComparable<T> constraint to ensure T can be compared.

    Sort method implements Bubble Sort, using either a custom comparer or the default comparer for T.

* 2.Person Class with IComparable Implementation:

    CompareTo method compares Person objects based on Age.

    ToString override helps with debugging and displaying results.
 */