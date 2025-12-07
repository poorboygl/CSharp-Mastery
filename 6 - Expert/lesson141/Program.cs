// ======================================================================
// PROGRAM ENTRY – CQRS + MEDIATOR DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== CQRS + MEDIATOR SAMPLE ===");

        var orderRepo = new OrderRepository();

        var createHandler = new CreateOrderHandler(orderRepo);
        var getHandler = new GetOrderHandler(orderRepo);

        var mediator = new Mediator();

        // REGISTER HANDLERS
        mediator.RegisterHandler<CreateOrderCommand, Guid>(createHandler);
        mediator.RegisterHandler<GetOrderQuery, Order>(getHandler);

        // DEMO: CREATE ORDER
        Console.WriteLine("\n>>> Sending CreateOrderCommand...");
        var orderId = mediator.Send<CreateOrderCommand, Guid>(new CreateOrderCommand("New Laptop Order"));
        Console.WriteLine($"Order created with Id: {orderId}");

        // DEMO: GET ORDER
        Console.WriteLine("\n>>> Sending GetOrderQuery...");
        var order = mediator.Send<GetOrderQuery, Order>(new GetOrderQuery(orderId));
        Console.WriteLine($"Order Found: {order.Id} - {order.Description}");

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// MARKER INTERFACES
// ======================================================================
public interface ICommand { }
public interface IQuery<TResult> { }


// ======================================================================
// GENERIC HANDLER INTERFACE
// ======================================================================
public interface IHandler<in TRequest, out TResult>
{
    TResult Handle(TRequest request);
}


// ======================================================================
// COMMANDS & QUERIES
// ======================================================================
public class CreateOrderCommand : ICommand
{
    public string Description { get; }

    public CreateOrderCommand(string description)
    {
        Description = description;
    }
}

public class GetOrderQuery : IQuery<Order>
{
    public Guid OrderId { get; }

    public GetOrderQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}


// ======================================================================
// DOMAIN ENTITY
// ======================================================================
public class Order
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
}


// ======================================================================
// REPOSITORY (IN-MEMORY)
// ======================================================================
public class OrderRepository
{
    private readonly List<Order> _orders = new();

    public void AddOrder(Order order) => _orders.Add(order);
    public Order GetOrder(Guid id) => _orders.Find(o => o.Id == id);
}


// ======================================================================
// HANDLERS
// ======================================================================

// CREATE ORDER HANDLER
public class CreateOrderHandler : IHandler<CreateOrderCommand, Guid>
{
    private readonly OrderRepository _orderRepository;

    public CreateOrderHandler(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Guid Handle(CreateOrderCommand command)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Description = command.Description
        };

        _orderRepository.AddOrder(order);
        Console.WriteLine("Order saved to repository.");
        return order.Id;
    }
}

// GET ORDER HANDLER
public class GetOrderHandler : IHandler<GetOrderQuery, Order>
{
    private readonly OrderRepository _orderRepository;

    public GetOrderHandler(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Order Handle(GetOrderQuery query)
    {
        return _orderRepository.GetOrder(query.OrderId);
    }
}


// ======================================================================
// MEDIATOR IMPLEMENTATION
// ======================================================================
public class Mediator
{
    private readonly Dictionary<Type, object> _handlers = new();

    public void RegisterHandler<TRequest, TResult>(IHandler<TRequest, TResult> handler)
    {
        _handlers[typeof(TRequest)] = handler;
    }

    public TResult Send<TRequest, TResult>(TRequest request)
    {
        if (_handlers.TryGetValue(typeof(TRequest), out var handler))
        {
            return ((IHandler<TRequest, TResult>)handler).Handle(request);
        }
        throw new InvalidOperationException("Handler not found");
    }
}

/*
 !=== CQRS + MEDIATOR SAMPLE ===

>>> Sending CreateOrderCommand...
Order saved to repository.
Order created with Id: 1b9df7fa-5b53-400f-a45f-079d7458c727

>>> Sending GetOrderQuery...
Order Found: 1b9df7fa-5b53-400f-a45f-079d7458c727 - New Laptop Order
 */

/*
* 1.Interfaces: ICommand and IQuery act as markers for commands and queries, while IHandler defines a general-purpose handler interface for processing requests.

* 2.Mediator: The Mediator class acts as a central point, registering and dispatching requests to their respective handlers.

* 3.Handlers: Each handler (CreateOrderHandler, GetOrderHandler) processes a specific command or query and uses OrderRepository to manage orders.
 */