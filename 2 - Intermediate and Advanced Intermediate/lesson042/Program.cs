public class Program
{
    static void Main()
    {
        var stock = new Stock();

        // subscribe to the event
        stock.PriceChanged += OnPriceChanged;

        Console.WriteLine("=== Stock PriceChanged Event Test ===");

        stock.UpdatePrice(100.5m);
        stock.UpdatePrice(102.75m);
        stock.UpdatePrice(102.75m); // No event (same price)
        stock.UpdatePrice(110.00m);

        Console.ReadKey();
    }

    // event handler
    private static void OnPriceChanged(object sender, decimal newPrice)
    {
        Console.WriteLine($"Price Updated: {newPrice}");
    }
}
public class Stock
{
    public decimal Price { get; private set; }
    public event EventHandler<decimal> PriceChanged;

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice != Price)
        {
            Price = newPrice;
            PriceChanged?.Invoke(this, newPrice);
        }
    }
}

/*
* 1.Event Declaration:

The PriceChanged event is declared using EventHandler<decimal>, passing the new price as event data.

* 2.Event Invocation:

UpdatePrice checks if the new price differs from the current price, updates the price, and then triggers PriceChanged.

* 3.Event Handling:

Subscribers to PriceChanged can handle the event and respond to price updates.
 */