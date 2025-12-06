public class Program
{
    static async Task Main()
    {
        Console.WriteLine("===== STOCK PRICE EVENT DEMO =====\n");

        var stock = new Stock { Symbol = "AAPL" };
        var subscriber = new StockSubscriber();

        subscriber.Subscribe(stock);

        stock.UpdatePrice(150.25m);
        stock.UpdatePrice(151.80m);
        stock.UpdatePrice(151.80m); // Không kích hoạt vì giá không thay đổi
        stock.UpdatePrice(149.99m);

        subscriber.Unsubscribe(stock);

        stock.UpdatePrice(140.00m); // Không còn hiển thị vì đã hủy đăng ký

        Console.ReadKey();
    }
}

public class Stock
{
    public required string Symbol { get; set; }
    public decimal Price { get; private set; }

    public event EventHandler<decimal> PriceChanged;

    public void UpdatePrice(decimal newPrice)
    {
        if (Price != newPrice)
        {
            Price = newPrice;
            PriceChanged?.Invoke(this, newPrice);
        }
    }
}

public class StockSubscriber
{
    public void Subscribe(Stock stock)
    {
        stock.PriceChanged += Notify;
    }

    public void Unsubscribe(Stock stock)
    {
        stock.PriceChanged -= Notify;
    }

    public void Notify(object sender, decimal newPrice)
    {
        var stock = (Stock)sender;
        Console.WriteLine($"Stock {stock.Symbol} price changed to {newPrice}");
    }
}

/*
 * 1.Lazy Initialization with Lazy<T>:

    _instance is initialized using Lazy<T>, which guarantees thread-safe, lazy initialization.

* 2.Double-Check Locking in GetResource:

    GetResource checks _isInitialized both outside and inside the lock to avoid multiple initializations.

    Thread.Sleep(500) simulates an expensive resource initialization that should happen only once.
 */