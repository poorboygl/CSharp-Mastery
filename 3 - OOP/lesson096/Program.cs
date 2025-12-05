public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Shopping Cart Demo ===\n");

        // Tạo giỏ hàng
        ShoppingCart cart = new ShoppingCart();

        // Thêm sản phẩm
        IProduct book = new PhysicalProduct("C# Programming Book", 500);
        IProduct ebook = new DigitalProduct("C# Programming eBook", 200);

        cart.AddProduct(book);
        cart.AddProduct(ebook);

        // In hóa đơn
        ReceiptPrinter printer = new ReceiptPrinter();
        printer.PrintReceipt(cart);

        Console.ReadKey();
    }
}
public interface IProduct
{
    string Name { get; }
    double Price { get; }
    double GetDiscountedPrice();
}

public class PhysicalProduct : IProduct
{
    public string Name { get; }
    public double Price { get; }

    public PhysicalProduct(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public double GetDiscountedPrice()
    {
        return Price;
    }
}

public class DigitalProduct : IProduct
{
    public string Name { get; }
    public double Price { get; }

    public DigitalProduct(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public double GetDiscountedPrice()
    {
        return Price * 0.9; // 10% discount
    }
}

public class ShoppingCart
{
    private readonly List<IProduct> _products = new List<IProduct>();

    public void AddProduct(IProduct product)
    {
        _products.Add(product);
    }

    public void RemoveProduct(IProduct product)
    {
        _products.Remove(product);
    }

    public double CalculateTotalPrice()
    {
        double total = 0;
        foreach (var product in _products)
        {
            total += product.GetDiscountedPrice();
        }
        return total;
    }

    public IEnumerable<IProduct> Products => _products;
}

public class ReceiptPrinter
{
    public void PrintReceipt(ShoppingCart cart)
    {
        Console.WriteLine("Receipt:");
        foreach (var product in cart.Products)
        {
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine($"Original Price: {product.Price:C}");
            Console.WriteLine($"Discounted Price: {product.GetDiscountedPrice():C}");
            Console.WriteLine();
        }
        Console.WriteLine($"Total Price: {cart.CalculateTotalPrice():C}");
    }
}

/*
* 1.IProduct Interface:

    Provides the structure for product classes, ensuring each product has a Name, Price, and a way to calculate GetDiscountedPrice.

* 2.PhysicalProduct and DigitalProduct:

    PhysicalProduct returns the full price in GetDiscountedPrice.

    DigitalProduct applies a 10% discount to the price.

* 3.ShoppingCart:

    Holds a list of IProduct items, demonstrating composition.

    CalculateTotalPrice sums the discounted prices for all products.

* 4.ReceiptPrinter:

    Iterates through each product in ShoppingCart, printing details about the name, original price, discounted price, and the total.
 
 */