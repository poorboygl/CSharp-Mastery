using System.Linq.Expressions;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Dynamic Query Builder Demo ===");

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200 },
            new Product { Id = 2, Name = "Mouse", Price = 25 },
            new Product { Id = 3, Name = "Keyboard", Price = 45 },
            new Product { Id = 4, Name = "Laptop", Price = 1500 }
        };

        // Build dynamic filter:
        // Name == "Laptop" AND Price > 1000
        var query = new DynamicQueryBuilder<Product>()
            .WhereEquals(nameof(Product.Name), "Laptop")
            .WhereGreaterThan(nameof(Product.Price), 1000)
            .Build();

        var results = products.Where(query);

        Console.WriteLine("\n=== Filter Result ===");
        foreach (var p in results)
        {
            Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Price: {p.Price}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class DynamicQueryBuilder<T>
{
    private readonly List<Expression<Func<T, bool>>> _filters = new();

    public DynamicQueryBuilder<T> WhereEquals(string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var equality = Expression.Equal(property, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
        _filters.Add(lambda);
        return this;
    }

    public DynamicQueryBuilder<T> WhereGreaterThan(string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(Convert.ChangeType(value, property.Type), property.Type);
        var greaterThan = Expression.GreaterThan(property, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(greaterThan, parameter);
        _filters.Add(lambda);
        return this;
    }

    public Func<T, bool> Build()
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression combined = null;

        foreach (var filter in _filters)
        {
            var invokedFilter = Expression.Invoke(filter, parameter);
            combined = combined == null ? invokedFilter : Expression.AndAlso(combined, invokedFilter);
        }

        return combined == null
            ? _ => true
            : Expression.Lambda<Func<T, bool>>(combined, parameter).Compile();
    }
}

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

/*
 === Dynamic Query Builder Demo ===

=== Filter Result ===
Id: 1, Name: Laptop, Price: 1200
Id: 4, Name: Laptop, Price: 1500
 */



/*
* 1.Expression Trees for Dynamic Filtering:

WhereEquals creates an equality expression using Expression.Equal.

WhereGreaterThan creates a greater-than expression using Expression.GreaterThan.

These expressions are stored in _filters and combined in Build.

* 2.Combining Expressions:

Build uses Expression.AndAlso to combine filters, resulting in a single Func<T, bool> that applies all conditions.
 
 */