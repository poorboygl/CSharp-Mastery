using System.Text.Json;
public class Program
{
    static void Main()
    {
        var inventory = new ProductInventory();

        // Add sample products
        inventory.AddProduct(new Product { Id = 1, Name = "Laptop", Price = 1200m, Quantity = 5 });
        inventory.AddProduct(new Product { Id = 2, Name = "Mouse", Price = 25m, Quantity = 20 });

        // Serialize
        string json = inventory.SerializeInventory();
        Console.WriteLine("=== Serialized Inventory (JSON) ===");
        Console.WriteLine(json);

        // Deserialize back into object
        var newInventory = new ProductInventory();
        newInventory.DeserializeInventory(json);

        Console.WriteLine("\n=== Deserialized Product List ===");
        foreach (var product in newInventory.GetProducts())
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Qty: {product.Quantity}");
        }

        Console.ReadKey();
    }
}

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class ProductInventory
{
    private List<Product> _products = new();

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public string SerializeInventory()
    {
        return JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
    }

    public void DeserializeInventory(string json)
    {
        _products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }

    public List<Product> GetProducts()
    {
        return new List<Product>(_products);
    }
}

/*
 === Serialized Inventory (JSON) ===
[
  {
    "Id": 1,
    "Name": "Laptop",
    "Price": 1200,
    "Quantity": 5
  },
  {
    "Id": 2,
    "Name": "Mouse",
    "Price": 25,
    "Quantity": 20
  }
]

=== Deserialized Product List ===
ID: 1, Name: Laptop, Price: 1200, Qty: 5
ID: 2, Name: Mouse, Price: 25, Qty: 20
 
 */

/*
* 1.In-Memory JSON Serialization and Deserialization:

    SerializeInventory uses JsonSerializer.Serialize to convert _products to a JSON string.

    DeserializeInventory uses JsonSerializer.Deserialize to load product data from a JSON string into _products.

* 2.Error Handling:

    DeserializeInventory ensures that _products is not null, using an empty list if deserialization fails.
 */