using System.Reflection;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== REPOSITORY PATTERN DEMO ===\n");
        // Tạo repository cho Product
        IRepository<Product> productRepo = new Repository<Product>();

        // Thêm sản phẩm
        productRepo.Add(new Product { Id = 1, Name = "Laptop", Price = 1500 });
        productRepo.Add(new Product { Id = 2, Name = "Mouse", Price = 25 });
        productRepo.Add(new Product { Id = 3, Name = "Keyboard", Price = 45 });

        // Lấy sản phẩm theo Id
        var p = productRepo.GetById(2);
        Console.WriteLine($"GetById(2): {p.Name} - ${p.Price}");

        // In toàn bộ sản phẩm
        Console.WriteLine("\n--- All Products ---");
        foreach (var product in productRepo.GetAll())
        {
            Console.WriteLine($"{product.Id}: {product.Name} - ${product.Price}");
        }

        // Xóa sản phẩm
        productRepo.Remove(p);

        // Danh sách sau khi xóa
        Console.WriteLine("\n--- After Remove ---");
        foreach (var product in productRepo.GetAll())
        {
            Console.WriteLine($"{product.Id}: {product.Name} - ${product.Price}");
        }

        Console.ReadKey();
    }
}
public interface IRepository<T>
{
    void Add(T item);
    T GetById(int id);
    void Remove(T item);
    List<T> GetAll();
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T GetById(int id)
    {
        var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            throw new InvalidOperationException("T must have a public Id property");
        }
        return _items.FirstOrDefault(item => (int)property.GetValue(item) == id);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public List<T> GetAll()
    {
        return _items.ToList();
    }
}

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

/*
* 1.Reflection for Id:

    GetById uses reflection to get the value of the Id property for each T instance.

* 2.CRUD Operations:

    Add and Remove add or remove items from _items.

    GetAll returns all items.
 
 */
