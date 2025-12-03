public class Program
{
    static void Main()
    {
        Console.WriteLine("===== STOCK PRICE CHANGE DEMO =====");

        var stock = new Stock { Symbol = "AAPL" };

        // Đăng ký lắng nghe sự kiện
        stock.PriceChanged += (sender, e) =>
        {
            Console.WriteLine($"[{((Stock)sender).Symbol}] Price changed: {e.OldPrice} --> {e.NewPrice}");
        };

        // Giả lập thay đổi giá
        stock.UpdatePrice(150.50m);
        stock.UpdatePrice(151.75m);
        stock.UpdatePrice(151.75m); // Không đổi → không kích hoạt event
        stock.UpdatePrice(149.00m);

        Console.WriteLine("===== END =====");
        Console.ReadKey();
    }
}


public class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; }
    public decimal NewPrice { get; }

    public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
    {
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}

public class Stock
{
    public string Symbol { get; set; } = string.Empty; // Initialized to avoid nullable warning
    public decimal Price { get; private set; }

    public event EventHandler<PriceChangedEventArgs>? PriceChanged; // Nullable to avoid warning

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice != Price)
        {
            decimal oldPrice = Price;
            Price = newPrice;
            OnPriceChanged(oldPrice, newPrice);
        }
    }

    protected virtual void OnPriceChanged(decimal oldPrice, decimal newPrice)
    {
        PriceChanged?.Invoke(this, new PriceChangedEventArgs(oldPrice, newPrice));
    }
}

/*
 ===== STOCK PRICE CHANGE DEMO =====
[AAPL] Price changed: 0 --> 150.50
[AAPL] Price changed: 150.50 --> 151.75
[AAPL] Price changed: 151.75 --> 149.00
===== END =====

 */

/*
* 1.Custom EventArgs Class (PriceChangedEventArgs):

Holds the OldPrice and NewPrice values to pass to the event handler.

* 2.Event in Stock Class:

PriceChanged is an event of type EventHandler<PriceChangedEventArgs> that is triggered when the stock price changes.

* 3.UpdatePrice Method:

If the new price differs from the current price, updates Price and calls OnPriceChanged.

* 4.OnPriceChanged Method:

Invokes the PriceChanged event, passing the current instance (this) and a new PriceChangedEventArgs instance with the old and new prices.
 
 */