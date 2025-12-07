public class Program
{
    static void Main()
    {
        // Demo: chạy quy trình đặt vé máy bay bằng Saga Pattern
        Console.WriteLine("=== FLIGHT BOOKING SAGA DEMO ===");

        var bookingService = new BookingService();
        var paymentService = new PaymentService();
        var notificationService = new NotificationService();

        ISagaOrchestrator orchestrator = new FlightBookingOrchestrator(
            bookingService,
            paymentService,
            notificationService
        );

        bool result = orchestrator.ExecuteSaga();

        Console.WriteLine(result
            ? "\n>>> FINAL RESULT: SUCCESS"
            : "\n>>> FINAL RESULT: FAILED");

        Console.ReadKey();
    }
}


// ======================================================================
// INTERFACES
// ======================================================================
public interface ISagaOrchestrator
{
    bool ExecuteSaga();
}

public interface IBookingService
{
    bool BookFlight();
    void CancelBooking();
}

public interface IPaymentService
{
    bool ProcessPayment();
    void RefundPayment();
}

public interface INotificationService
{
    void SendNotification();
}


// ======================================================================
// SERVICES IMPLEMENTATION
// ======================================================================

// ------------------------
// BOOKING SERVICE
// ------------------------
public class BookingService : IBookingService
{
    public bool BookFlight()
    {
        Console.WriteLine("Booking flight...");
        return true;
    }

    public void CancelBooking()
    {
        Console.WriteLine("Cancelling flight booking...");
    }
}

// ------------------------
// PAYMENT SERVICE
// ------------------------
public class PaymentService : IPaymentService
{
    public bool ProcessPayment()
    {
        Console.WriteLine("Processing payment...");

        // Random mô phỏng thành công/thất bại
        bool success = new Random().Next(2) == 0;

        if (!success)
            Console.WriteLine("Payment failed!");

        return success;
    }

    public void RefundPayment()
    {
        Console.WriteLine("Refunding payment...");
    }
}

// ------------------------
// NOTIFICATION SERVICE
// ------------------------
public class NotificationService : INotificationService
{
    public void SendNotification()
    {
        Console.WriteLine("Sending booking confirmation notification...");
    }
}


// ======================================================================
// ORCHESTRATOR (SAGA IMPLEMENTATION)
// ======================================================================
public class FlightBookingOrchestrator : ISagaOrchestrator
{
    private readonly IBookingService _bookingService;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;

    public FlightBookingOrchestrator(
        IBookingService bookingService,
        IPaymentService paymentService,
        INotificationService notificationService)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _notificationService = notificationService;
    }

    public bool ExecuteSaga()
    {
        Console.WriteLine("\nStarting flight booking SAGA transaction...");

        // STEP 1: Book flight
        if (!_bookingService.BookFlight())
            return false;

        // STEP 2: Process payment
        if (!_paymentService.ProcessPayment())
        {
            Console.WriteLine("Saga detected failure --> Rolling back...");
            _bookingService.CancelBooking();
            return false;
        }

        // STEP 3: Send notification
        _notificationService.SendNotification();

        Console.WriteLine("Flight booking completed successfully!");
        return true;
    }
}

/*
* 1.FlightBookingOrchestrator: The main orchestrator that coordinates the transaction steps, calling each service in sequence. It rolls back by calling CancelBooking if ProcessPayment fails.

* 2.BookingService, PaymentService, and NotificationService: Each service performs its designated action, with BookingService and PaymentService also implementing rollback methods (CancelBooking, RefundPayment).
 */
