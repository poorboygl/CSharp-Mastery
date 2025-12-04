public class Program
{
    static void Main()
    {
        Console.WriteLine("=== ORDER MANAGEMENT SYSTEM ===\n");

        // Create products
        var p1 = new Product { Name = "Laptop", Price = 1500 };
        var p2 = new Product { Name = "Mouse", Price = 25 };

        // Create order
        var order = new Order();
        order.AddItem(p1, 2);   // 2 laptops
        order.AddItem(p2, 3);   // 3 mice

        // Print items
        Console.WriteLine("Order Items:");
        Console.WriteLine($" - {p1.Name} x2 = {p1.Price * 2}");
        Console.WriteLine($" - {p2.Name} x3 = {p2.Price * 3}");

        // Print total
        Console.WriteLine($"\nTotal Order Price: {order.TotalPrice}");

        // Test negative price
        Console.WriteLine("\n--- Testing Negative Price ---");
        try
        {
            p1.Price = -10;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Test invalid quantity
        Console.WriteLine("\n--- Testing Invalid Quantity ---");
        try
        {
            var wrong = new OrderItem(p2, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.ReadKey();
    }
}

public class Product
{
    public required string Name { get; set; }

    private double _price;
    public double Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            _price = value;
        }
    }
}

public class OrderItem
{
    public Product Product { get; }
    private int _quantity;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Quantity must be at least 1.");
            }
            _quantity = value;
        }
    }

    public OrderItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public double GetTotalPrice()
    {
        return Product.Price * Quantity;
    }
}

public class Order
{
    private List<OrderItem> _items = new List<OrderItem>();

    public void AddItem(Product product, int quantity)
    {
        var orderItem = new OrderItem(product, quantity);
        _items.Add(orderItem);
    }

    public double TotalPrice
    {
        get
        {
            double total = 0;
            foreach (var item in _items)
            {
                total += item.GetTotalPrice();
            }
            return total;
        }
    }
}

/*
* 1.Product Class:

    The Price property has validation to ensure the price is not negative.

* 2.OrderItem Class:

    The Quantity property has validation to ensure at least one unit is ordered.

    GetTotalPrice calculates the price for the item by multiplying Product.Price and Quantity.

* 3.Order Class:

    _items is a private list that stores the order items.

    AddItem allows adding products to the order with specified quantities.

    TotalPrice calculates the overall price of all items in the order by summing each item’s total price.
 */