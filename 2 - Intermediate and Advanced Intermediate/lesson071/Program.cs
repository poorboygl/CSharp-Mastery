public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Dynamic Filter Builder Demo ===");

        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Price = 1200 },
            new() { Id = 2, Name = "Mouse", Price = 25 },
            new() { Id = 3, Name = "Keyboard", Price = 45 },
            new() { Id = 4, Name = "Monitor", Price = 300 },
            new() { Id = 5, Name = "Tablet", Price = 500 }
        };

        Console.WriteLine("\nOriginal Product List:");
        products.ForEach(p => Console.WriteLine(p));

        // Build filter: Price > 50 AND Name contains 'o'
        var filterBuilder = new FilterBuilder<Product>()
            .Where(p => p.Price > 50)
            .Where(p => p.Name.Contains("o", StringComparison.OrdinalIgnoreCase));

        var filteredProducts = filterBuilder.ApplyFilter(products);

        Console.WriteLine("\nFiltered Products (Price > 50 AND Name contains 'o'):");
        filteredProducts.ForEach(p => Console.WriteLine(p));

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class FilterBuilder<T>
{
    private readonly List<Func<T, bool>> _predicates = new();

    public FilterBuilder<T> Where(Func<T, bool> predicate)
    {
        _predicates.Add(predicate);
        return this;
    }

    public Func<T, bool> Build()
    {
        return item => _predicates.All(predicate => predicate(item));
    }

    public List<T> ApplyFilter(List<T> items)
    {
        var filter = Build();
        return items.Where(filter).ToList();
    }
}

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Name} - ${Price}";
    }
}
/*
 === Dynamic Filter Builder Demo ===

Original Product List:
Laptop - $1200
Mouse - $25
Keyboard - $45
Monitor - $300
Tablet - $500

Filtered Products (Price > 50 AND Name contains 'o'):
Laptop - $1200
Monitor - $300

Press any key to exit...

 
 */

/*
* 1.Dynamic Predicate Building:

    Where adds predicates to _predicates.

    Build combines all predicates with logical AND (&&) using All to ensure all conditions are met for each item.

* 2.Filter Application:

    ApplyFilter retrieves the combined filter function from Build, then uses it with LINQ’s Where to get the filtered results.
 
 */
