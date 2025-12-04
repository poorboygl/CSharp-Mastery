public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Order State Machine Demo ===");

        var order = new Order();

        Console.WriteLine("\nCurrent flow:");
        order.Process();  // Pending → Processed
        order.Ship();     // Processed → Shipped
        order.Deliver();  // Shipped → Delivered

        Console.WriteLine("\nTrying invalid operation after delivery:");
        try
        {
            order.Ship();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public interface IOrderState
{
    void Process(Order order);
    void Ship(Order order);
    void Deliver(Order order);
}

public class Order
{
    private IOrderState _state;

    public Order()
    {
        _state = new PendingState();
    }

    public void Process() => _state.Process(this);
    public void Ship() => _state.Ship(this);
    public void Deliver() => _state.Deliver(this);

    public void ChangeState(IOrderState state) => _state = state;
}

public class PendingState : IOrderState
{
    public void Process(Order order)
    {
        Console.WriteLine("Order processed.");
        order.ChangeState(new ProcessedState());
    }

    public void Ship(Order order)
    {
        throw new InvalidOperationException("Order must be processed before shipping.");
    }

    public void Deliver(Order order)
    {
        throw new InvalidOperationException("Order must be processed and shipped before delivery.");
    }
}

public class ProcessedState : IOrderState
{
    public void Process(Order order)
    {
        throw new InvalidOperationException("Order has already been processed.");
    }

    public void Ship(Order order)
    {
        Console.WriteLine("Order shipped.");
        order.ChangeState(new ShippedState());
    }

    public void Deliver(Order order)
    {
        throw new InvalidOperationException("Order must be shipped before delivery.");
    }
}

public class ShippedState : IOrderState
{
    public void Process(Order order)
    {
        throw new InvalidOperationException("Order has already been processed.");
    }

    public void Ship(Order order)
    {
        throw new InvalidOperationException("Order has already been shipped.");
    }

    public void Deliver(Order order)
    {
        Console.WriteLine("Order delivered.");
        order.ChangeState(new DeliveredState());
    }
}

public class DeliveredState : IOrderState
{
    public void Process(Order order)
    {
        throw new InvalidOperationException("Order has already been delivered.");
    }

    public void Ship(Order order)
    {
        throw new InvalidOperationException("Order has already been delivered.");
    }

    public void Deliver(Order order)
    {
        throw new InvalidOperationException("Order has already been delivered.");
    }
}

/*
* 1.Order State Management:

    Order starts in PendingState.

    Each state class implements the transitions allowed from that state, throwing exceptions if a transition is invalid.

* 2.State Transitions:

    PendingState transitions to ProcessedState upon Process.

    ProcessedState transitions to ShippedState upon Ship.

    ShippedState transitions to DeliveredState upon Deliver.

    DeliveredState does not allow any further transitions.
 */
