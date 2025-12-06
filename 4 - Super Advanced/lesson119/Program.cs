public class Program
{
    static void Main()
    {
        Console.Title = "Observer Pattern Demo";
        Console.WriteLine("=== OBSERVER PATTERN: STOCK PRICE MONITOR ===\n");

        var stock = new Stock();

        var investorA = new Investor("Alice");
        var investorB = new Investor("Bob");
        var investorC = new Investor("Charlie");

        stock.Attach(investorA);
        stock.Attach(investorB);
        stock.Attach(investorC);

        Console.WriteLine("Setting stock price to 100...");
        stock.Price = 100;
        Console.WriteLine();

        Console.WriteLine("Setting stock price to 105...");
        stock.Price = 105;
        Console.WriteLine();

        Console.WriteLine("Detaching Bob...\n");
        stock.Detach(investorB);

        Console.WriteLine("Setting stock price to 110...");
        stock.Price = 110;

        Console.ReadKey();
    }
}

public interface IObserver
{
    void Update(decimal price);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

public class Stock : ISubject
{
    private readonly List<IObserver> _observers = new();
    private decimal _price;

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                Notify();
            }
        }
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_price);
        }
    }
}

public class Investor : IObserver
{
    public string Name { get; }

    public Investor(string name)
    {
        Name = name;
    }

    public void Update(decimal price)
    {
        Console.WriteLine($"Investor {Name} notified. New stock price: {price}");
    }
}

/*
* 1.Stock Class with Price Notification:

    Stock manages a list of observers and notifies them whenever Price changes.

    Notify iterates over all observers, calling Update on each to send the latest price.

* 2.Investor Class Receiving Updates:

    Investor implements Update, printing a message with the new price and investor’s name.
 */