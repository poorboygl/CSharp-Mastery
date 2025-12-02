public class Program
{
    static void Main()
    {
        Console.WriteLine("===== ORDER PROCESSOR DEMO =====");

        IPaymentService paymentService = new MockPaymentService();
        IOrderRepository orderRepository = new MockOrderRepository();

        var processor = new OrderProcessor(paymentService, orderRepository);

        var order = new Order
        {
            OrderId = 1,
            Amount = 250.50m
        };

        processor.ProcessOrder(order);

        Console.WriteLine("===== END OF PROGRAM =====");

        Console.ReadKey();
    }
}
public interface IPaymentService
{
    bool ProcessPayment(decimal amount);
}

public interface IOrderRepository
{
    void SaveOrder(Order order);
}

public class Order
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}

public class OrderProcessor
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderRepository _orderRepository;

    public OrderProcessor(IPaymentService paymentService, IOrderRepository orderRepository)
    {
        _paymentService = paymentService;
        _orderRepository = orderRepository;
    }

    public void ProcessOrder(Order order)
    {
        if (_paymentService.ProcessPayment(order.Amount))
        {
            _orderRepository.SaveOrder(order);
        }
        else
        {
            Console.WriteLine("Payment failed. Order not processed.");
        }
    }
}

public class MockPaymentService : IPaymentService
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing payment of ${amount}");
        return true;
    }
}

public class MockOrderRepository : IOrderRepository
{
    public void SaveOrder(Order order)
    {
        Console.WriteLine($"Order {order.OrderId} saved successfully.");
    }
}

/*
 ===== ORDER PROCESSOR DEMO =====
Processing payment of $250.50
Order 1 saved successfully.
===== END OF PROGRAM =====

 */

/*
* 1.Order Processing Logic:

    OrderProcessor checks if the payment succeeds before saving the order. This logic is decoupled from specific implementations by using interfaces.

* 2.Mock Implementations:

    MockPaymentService simulates a successful payment.

    MockOrderRepository simulates saving an order without requiring a real database.
 */