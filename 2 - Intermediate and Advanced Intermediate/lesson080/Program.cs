public class Program
{
    static void Main()
    {
        var repository = new GenericRepository<Product>();
        var service = new Service<Product>(repository);

        // Add sample data
        service.AddEntity(new Product { Id = 1, Name = "Laptop" });
        service.AddEntity(new Product { Id = 2, Name = "Smartphone" });
        service.AddEntity(new Product { Id = 3, Name = "Tablet" });

        Console.WriteLine("=== All Products ===");
        foreach (var product in service.GetAllEntities())
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Get Product By ID (ID = 2) ===");
        var p = service.GetEntity(2);
        if (p != null)
            Console.WriteLine($"Found: ID: {p.Id}, Name: {p.Name}");
        else
            Console.WriteLine("Product not found.");

        Console.WriteLine();
        Console.WriteLine("=== Remove Product (ID = 1) ===");
        service.RemoveEntity(1);

        Console.WriteLine("=== Products After Removal ===");
        foreach (var product in service.GetAllEntities())
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
        }

        Console.ReadKey();
    }
}
public interface IEntity
{
    int Id { get; set; }
}

public interface IRepository<T> where T : IEntity
{
    void Add(T entity);
    T Get(int id);
    IEnumerable<T> GetAll();
    void Remove(int id);
}

public class GenericRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _entities = new();

    public void Add(T entity)
    {
        _entities.Add(entity);
    }

    public T Get(int id)
    {
        return _entities.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _entities;
    }

    public void Remove(int id)
    {
        var entity = Get(id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }
    }
}

public class Service<T> where T : IEntity
{
    private readonly IRepository<T> _repository;

    public Service(IRepository<T> repository)
    {
        _repository = repository;
    }

    public void AddEntity(T entity) => _repository.Add(entity);
    public T GetEntity(int id) => _repository.Get(id);
    public IEnumerable<T> GetAllEntities() => _repository.GetAll();
    public void RemoveEntity(int id) => _repository.Remove(id);
}

public class Product : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

/*
 === All Products ===
ID: 1, Name: Laptop
ID: 2, Name: Smartphone
ID: 3, Name: Tablet

=== Get Product By ID (ID = 2) ===
Found: ID: 2, Name: Smartphone

=== Remove Product (ID = 1) ===
=== Products After Removal ===
ID: 2, Name: Smartphone
ID: 3, Name: Tablet
 */

/*
* 1.Generic Repository with Constraints:

    GenericRepository<T> enforces that T implements IEntity by specifying where T : IEntity.

    Methods like Get and Remove use the Id property to find entities.

* 2.Service with Dependency Injection:

    Service<T> uses IRepository<T> to interact with the data layer, allowing for flexible dependency injection.
 */