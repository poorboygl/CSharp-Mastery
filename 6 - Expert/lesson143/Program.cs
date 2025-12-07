// ======================================================================
// PROGRAM ENTRY – ORDER STATE MACHINE DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== ORDER STATE MACHINE DEMO ===");

        IOrderStateMachine order = new OrderStateMachine();

        Console.WriteLine($"\nInitial State: {order.State}");

        Console.WriteLine("\n>>> Processing Payment...");
        order.ProcessPayment();
        Console.WriteLine($"Current State: {order.State}");

        Console.WriteLine("\n>>> Shipping Order...");
        order.ShipOrder();
        Console.WriteLine($"Current State: {order.State}");

        Console.WriteLine("\n>>> Delivering Order...");
        order.DeliverOrder();
        Console.WriteLine($"Final State: {order.State}");

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// ORDER STATES ENUM
// ======================================================================
public enum OrderState
{
    Created,
    PaymentPending,
    Shipped,
    Delivered
}


// ======================================================================
// STATE MACHINE INTERFACE
// ======================================================================
public interface IOrderStateMachine
{
    OrderState State { get; }
    void ProcessPayment();
    void ShipOrder();
    void DeliverOrder();
}


// ======================================================================
// STATE MACHINE IMPLEMENTATION
// ======================================================================
public class OrderStateMachine : IOrderStateMachine
{
    public OrderState State { get; private set; } = OrderState.Created;

    public void ProcessPayment()
    {
        if (State != OrderState.Created)
            throw new InvalidOperationException("Payment can only be processed from Created state.");

        State = OrderState.PaymentPending;
        Console.WriteLine("Order state changed to PaymentPending.");
    }

    public void ShipOrder()
    {
        if (State != OrderState.PaymentPending)
            throw new InvalidOperationException("Order can only be shipped after payment.");

        State = OrderState.Shipped;
        Console.WriteLine("Order state changed to Shipped.");
    }

    public void DeliverOrder()
    {
        if (State != OrderState.Shipped)
            throw new InvalidOperationException("Order can only be delivered after it has been shipped.");

        State = OrderState.Delivered;
        Console.WriteLine("Order state changed to Delivered.");
    }
}

/*
 === ORDER STATE MACHINE DEMO ===

Initial State: Created

>>> Processing Payment...
Order state changed to PaymentPending.
Current State: PaymentPending

>>> Shipping Order...
Order state changed to Shipped.
Current State: Shipped

>>> Delivering Order...
Order state changed to Delivered.
Final State: Delivered

=== DONE ===

 */

/*
* 1.OrderState Enum: Represents the various states of an order.

* 2.OrderStateMachine: Implements the state machine pattern by encapsulating order states and transitions. Each method verifies the current state and only allows valid transitions.

    ProcessPayment transitions the order from Created to PaymentPending.

    ShipOrder transitions it from PaymentPending to Shipped.

    DeliverOrder transitions it from Shipped to Delivered.
 */