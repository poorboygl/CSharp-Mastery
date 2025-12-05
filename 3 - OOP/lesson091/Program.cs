public class Program
{
    static void Main()
    {
        Console.WriteLine("=== CUSTOM COLLECTION DEMO ===\n");

        var collection = new CustomCollection();

        // Add items
        collection.Add("Apple");
        collection.Add("Banana");
        collection.Add("Cherry");

        Console.WriteLine("Items added:");
        Console.WriteLine($"Item 0: {collection[0]}");
        Console.WriteLine($"Item 1: {collection[1]}");
        Console.WriteLine($"Item 2: {collection[2]}");

        Console.WriteLine($"\nTotal items: {collection.Count}");

        // Remove an item
        collection.Remove("Banana");
        Console.WriteLine("\nRemoved 'Banana'.");

        Console.WriteLine($"Total items after removal: {collection.Count}");
        Console.WriteLine($"Item 0: {collection[0]}");
        Console.WriteLine($"Item 1: {collection[1]}");

        Console.ReadKey();
    }
}


public class CustomCollection
{
    private List<string> _items = new List<string>();

    public void Add(string item)
    {
        _items.Add(item);
    }

    public void Remove(string item)
    {
        _items.Remove(item);
    }

    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
            return _items[index];
        }
    }

    public int Count => _items.Count;
}


/*
 * 1.Private Field for Storage:

    _items is a List<string> used to store the collection’s elements.

* 2.Add and Remove Methods:

    Add method adds a new item to _items.

    Remove method removes the specified item if it exists.

* 3.Indexer:

    public string this[int index] allows access to items using an index.

    Checks if the index is within bounds; if not, it throws an IndexOutOfRangeException.

* 4.Count Property:

    Count provides the number of elements in the collection by returning _items.Count.
 
 */