public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Generic Sorter Demo ===");

        // --- Sorting integers ---
        var intSorter = new GenericSorter<int>();
        var numbers = new List<int> { 5, 1, 9, 3, 7 };

        Console.WriteLine("\nOriginal integer list:");
        numbers.ForEach(n => Console.Write(n + " "));

        intSorter.Sort(numbers);
        Console.WriteLine("\nSorted ascending:");
        numbers.ForEach(n => Console.Write(n + " "));

        intSorter.ReverseSort(numbers);
        Console.WriteLine("\nSorted descending:");
        numbers.ForEach(n => Console.Write(n + " "));


        // --- Sorting persons ---
        var personSorter = new GenericSorter<Person>();
        var people = new List<Person>
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 22 },
            new Person { Name = "John", Age = 40 },
        };

        Console.WriteLine("\n\nOriginal people list:");
        people.ForEach(p => Console.WriteLine(p));

        personSorter.Sort(people);
        Console.WriteLine("\nPeople sorted by Age (ascending):");
        people.ForEach(p => Console.WriteLine(p));

        personSorter.ReverseSort(people);
        Console.WriteLine("\nPeople sorted by Age (descending):");
        people.ForEach(p => Console.WriteLine(p));


        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}


public class GenericSorter<T> where T : IComparable<T>
{
    public void Sort(List<T> items, IComparer<T> comparer = null)
    {
        items.Sort(comparer ?? Comparer<T>.Default);
    }

    public void ReverseSort(List<T> items, IComparer<T> comparer = null)
    {
        items.Sort(comparer ?? Comparer<T>.Default);
        items.Reverse();
    }
}

public class Person : IComparable<Person>
{
    public required string Name { get; set; }
    public int Age { get; set; }

    public int CompareTo(Person other)
    {
        if (other == null) return 1;
        return Age.CompareTo(other.Age);
    }

    public override string ToString()
    {
        return $"{Name}, Age {Age}";
    }
}

/*
     Original integer list:
    5 1 9 3 7
    Sorted ascending:
    1 3 5 7 9
    Sorted descending:
    9 7 5 3 1

    Original people list:
    Alice, Age 30
    Bob, Age 22
    John, Age 40

    People sorted by Age (ascending):
    Bob, Age 22
    Alice, Age 30
    John, Age 40

    People sorted by Age (descending):
    John, Age 40
    Alice, Age 30
    Bob, Age 22
 */

/*
* 1.GenericSorter with Custom Sort Options:

    Sort sorts the items in ascending order using the provided comparer or the default comparison for T.

    ReverseSort sorts items in ascending order and then reverses the list if no comparer is provided, achieving descending order.

* 2.Person Comparison:

Person implements IComparable<Person> by comparing the Age property.

CompareTo allows sorting by age in ascending order, which is the default behavior if no custom comparer is provided.
 
 */