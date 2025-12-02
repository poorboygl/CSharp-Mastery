public class Program
{
    static void Main()
    {
        var products = new List<Product>
        {
            new() { Name = "Laptop", Price = 1000m },
            new() { Name = "Mouse", Price = 25m },
            new() { Name = "Keyboard", Price = 45m },
            new() { Name = "Monitor", Price = 250m },
            new() { Name = "Mouse Pad", Price = 25m }
        };

        Console.WriteLine("=== ProductSorter Result ===");
        Console.WriteLine("Before Sorting:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Name} - ${p.Price}");
        }

        ProductSorter.SortProducts(products);

        Console.WriteLine("\nAfter Sorting (by Price, then Name):");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Name} - ${p.Price}");
        }

        Console.ReadKey();
    }
}


public class Product : IComparable<Product>
{
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public int CompareTo(Product other)
    {
        if (other == null) return 1;

        int priceComparison = Price.CompareTo(other.Price);
        if (priceComparison != 0)
        {
            return priceComparison;
        }

        return Name.CompareTo(other.Name);
    }
}

public class ProductSorter
{
    public static void SortProducts(List<Product> products)
    {
        products.Sort();
    }
}

/*
* 1.Implement CompareTo:

    Compare Price values; if they differ, return the comparison result.

    If prices are the same, compare by Name.

* 2.Sort with List.Sort:

    Calling SortProducts will sort products in ascending order based on the CompareTo logic.
 */