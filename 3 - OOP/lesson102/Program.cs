public class Program
{
    static void Main()
    {
        Console.WriteLine("===== STOCK ALERT SYSTEM DEMO =====\n");

        var stock = new Stock("AAPL", 150, 155);
        var subscriber = new StockAlertSubscriber("Investor 1");

        subscriber.Subscribe(stock);

        Console.WriteLine("Updating prices...\n");

        stock.UpdatePrice(152);   // chưa vượt ngưỡng → không báo
        stock.UpdatePrice(156);   // vượt ngưỡng → báo
        stock.UpdatePrice(154);   // xuống dưới ngưỡng → báo
        stock.UpdatePrice(160);   // vượt lại → báo

        Console.WriteLine("\n===== END OF DEMO =====");

        Console.ReadKey();
    }
}

public class Stock
{
    public string Symbol { get; set; }
    public double Price { get; private set; }
    public double AlertThreshold { get; set; }

    // Define the delegate and event
    public delegate void PriceChangedEventHandler(object sender, double newPrice);
    public event PriceChangedEventHandler PriceChanged;

    public Stock(string symbol, double initialPrice, double alertThreshold)
    {
        Symbol = symbol;
        Price = initialPrice;
        AlertThreshold = alertThreshold;
    }

    public void UpdatePrice(double newPrice)
    {
        if ((Price < AlertThreshold && newPrice >= AlertThreshold) ||
            (Price >= AlertThreshold && newPrice < AlertThreshold))
        {
            // Trigger the event if the price crosses the threshold
            OnPriceChanged(newPrice);
        }
        Price = newPrice;
    }

    protected virtual void OnPriceChanged(double newPrice)
    {
        PriceChanged?.Invoke(this, newPrice);
    }
}

public class StockAlertSubscriber
{
    public string SubscriberName { get; set; }

    public StockAlertSubscriber(string name)
    {
        SubscriberName = name;
    }

    public void Subscribe(Stock stock)
    {
        stock.PriceChanged += OnPriceChanged;
    }

    public void Unsubscribe(Stock stock)
    {
        stock.PriceChanged -= OnPriceChanged;
    }

    private void OnPriceChanged(object sender, double newPrice)
    {
        var stock = (Stock)sender;
        Console.WriteLine($"Alert! {stock.Symbol} has reached the price of {newPrice}");
    }
}


/*
* 1.Stock Class with Event:

    PriceChangedEventHandler defines the delegate type for the event.

    PriceChanged event is triggered in OnPriceChanged if the price crosses the threshold.

    UpdatePrice checks if the new price crosses the threshold, and if so, calls OnPriceChanged.

* 2.StockAlertSubscriber Class:

    Subscribe method subscribes to a stock’s PriceChanged event.

    OnPriceChanged handles the event, displaying an alert when the price crosses the threshold.
 */