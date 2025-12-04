public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Stock Price Event Demo ===");

        var stock = new Stock(threshold: 100m);
        var monitor = new StockMonitor(stock);

        Console.WriteLine("\nUpdating prices...\n");

        stock.UpdatePrice(80m);
        stock.UpdatePrice(95m);
        stock.UpdatePrice(100m);  // threshold reached
        stock.UpdatePrice(120m);  // still above threshold

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
public class Stock
{
    public event EventHandler<decimal> PriceChanged;
    public event EventHandler ThresholdReached;

    private decimal _price;
    private decimal _threshold;

    public Stock(decimal threshold)
    {
        _threshold = threshold;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (_price != newPrice)
        {
            _price = newPrice;
            PriceChanged?.Invoke(this, _price);

            if (_price >= _threshold)
            {
                ThresholdReached?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

public class StockMonitor
{
    public StockMonitor(Stock stock)
    {
        stock.PriceChanged += OnPriceChanged;
        stock.ThresholdReached += OnThresholdReached;
    }

    private void OnPriceChanged(object sender, decimal newPrice)
    {
        Console.WriteLine($"Price updated to: {newPrice}");
    }

    private void OnThresholdReached(object sender, EventArgs e)
    {
        Console.WriteLine("Price threshold reached!");
    }
}

/*
* 1.Event Declaration:

    PriceChanged is raised whenever the price changes.

    ThresholdReached is raised when the price meets or exceeds the threshold.

* 2.Stock Monitor:

    StockMonitor subscribes to both PriceChanged and ThresholdReached.

    Displays a message when the price changes or reaches the threshold.
 
 */