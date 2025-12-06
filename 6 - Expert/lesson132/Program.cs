public class Program
{
    static void Main()
    {
        Console.WriteLine("=== TRACE LOGGING WITH CORRELATION ID DEMO ===\n");

        // Shared trace context for the whole system
        ITracer tracer = new TraceContext();
        ITraceLogger logger = new ConsoleLogger(tracer);

        var inventory = new InventoryService(tracer, logger);
        var payment = new PaymentService(tracer, logger, inventory);
        var order = new OrderService(tracer, logger, payment);

        // Simulate order
        order.PlaceOrder();

        Console.WriteLine("\n=== END OF PROGRAM ===");
        Console.ReadKey();
    }
}


// ======================================================================
// INTERFACES
// ======================================================================
public interface ITraceLogger
{
    void LogInfo(string message);
    void LogError(string message);
}

public interface ITracer
{
    string GetCorrelationId();
    void SetCorrelationId(string correlationId);
}

// ======================================================================
// TRACE CONTEXT (ThreadStatic correlation ID)
// ======================================================================
public class TraceContext : ITracer
{
    [ThreadStatic] private static string _correlationId;

    public string GetCorrelationId() => _correlationId ??= Guid.NewGuid().ToString();

    public void SetCorrelationId(string correlationId) => _correlationId = correlationId;
}

// ======================================================================
// CONSOLE LOGGER
// ======================================================================
public class ConsoleLogger : ITraceLogger
{
    private readonly ITracer _tracer;

    public ConsoleLogger(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO]  [{_tracer.GetCorrelationId()}] {message}");
    }

    public void LogError(string message)
    {
        Console.WriteLine($"[ERROR] [{_tracer.GetCorrelationId()}] {message}");
    }
}

// ======================================================================
// ORDER SERVICE
// ======================================================================
public class OrderService
{
    private readonly ITracer _tracer;
    private readonly ITraceLogger _logger;
    private readonly PaymentService _paymentService;

    public OrderService(ITracer tracer, ITraceLogger logger, PaymentService paymentService)
    {
        _tracer = tracer;
        _logger = logger;
        _paymentService = paymentService;
    }

    public void PlaceOrder()
    {
        // New correlation ID for this order
        _tracer.SetCorrelationId(Guid.NewGuid().ToString());
        _logger.LogInfo("Placing order...");
        _paymentService.ProcessPayment();
    }
}

// ======================================================================
// PAYMENT SERVICE
// ======================================================================
public class PaymentService
{
    private readonly ITracer _tracer;
    private readonly ITraceLogger _logger;
    private readonly InventoryService _inventoryService;

    public PaymentService(ITracer tracer, ITraceLogger logger, InventoryService inventoryService)
    {
        _tracer = tracer;
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public void ProcessPayment()
    {
        _logger.LogInfo("Processing payment...");
        _inventoryService.UpdateInventory();
    }
}

// ======================================================================
// INVENTORY SERVICE
// ======================================================================
public class InventoryService
{
    private readonly ITracer _tracer;
    private readonly ITraceLogger _logger;

    public InventoryService(ITracer tracer, ITraceLogger logger)
    {
        _tracer = tracer;
        _logger = logger;
    }

    public void UpdateInventory()
    {
        _logger.LogInfo("Updating inventory...");
    }
}


/*
 === TRACE LOGGING WITH CORRELATION ID DEMO ===

[INFO]  [2fbc3ea3-2359-4247-8613-bea34285867f] Placing order...
[INFO]  [2fbc3ea3-2359-4247-8613-bea34285867f] Processing payment...
[INFO]  [2fbc3ea3-2359-4247-8613-bea34285867f] Updating inventory...

=== END OF PROGRAM ===
 */

/*
* 1.TraceContext: Maintains correlation ID across service calls, ensuring it’s accessible throughout the request’s lifecycle.

* 2.ConsoleLogger: Logs messages with the correlation ID, allowing logs to be traced back to a specific request.

* 3.OrderService, PaymentService, InventoryService: Each service performs its task and logs the operation with the shared correlation ID, simulating a full order process.
 */