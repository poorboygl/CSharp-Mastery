// ======================================================================
// BOOKING SAGA DEMO - EVENT DRIVEN WORKFLOW + COMPENSATION
// ======================================================================

public class Program
{
    static void Main()
    {
        // ==================================================================
        // DEMO – HAPPY PATH + FAILURE PATH
        // ==================================================================

        var dispatcher = new EventDispatcher();
        var saga = new BookingSaga(dispatcher);

        var bookingId = Guid.NewGuid();

        Console.WriteLine("=== START BOOKING ===");
        saga.StartBooking(bookingId);

        Console.WriteLine();

        Console.WriteLine("=== SIMULATE FAILURE ===");
        saga.HandleFailure(bookingId);

        Console.ReadKey();
    }
}

// ======================================================================
// EVENT DEFINITIONS
// ======================================================================

public interface IEvent { }

// ---------------- SUCCESS EVENTS ----------------

public class FlightReservedEvent : IEvent
{
    public Guid BookingId { get; }
    public FlightReservedEvent(Guid bookingId) => BookingId = bookingId;
}

public class HotelBookedEvent : IEvent
{
    public Guid BookingId { get; }
    public HotelBookedEvent(Guid bookingId) => BookingId = bookingId;
}

public class PaymentProcessedEvent : IEvent
{
    public Guid BookingId { get; }
    public PaymentProcessedEvent(Guid bookingId) => BookingId = bookingId;
}

// ---------------- COMPENSATION EVENTS ----------------

public class CancelFlightEvent : IEvent
{
    public Guid BookingId { get; }
    public CancelFlightEvent(Guid bookingId) => BookingId = bookingId;
}

public class CancelHotelEvent : IEvent
{
    public Guid BookingId { get; }
    public CancelHotelEvent(Guid bookingId) => BookingId = bookingId;
}

public class RefundPaymentEvent : IEvent
{
    public Guid BookingId { get; }
    public RefundPaymentEvent(Guid bookingId) => BookingId = bookingId;
}

// ======================================================================
// EVENT DISPATCHER (PUB/SUB)
// ======================================================================

public class EventDispatcher
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var type = typeof(TEvent);
        if (_subscribers.ContainsKey(type))
        {
            foreach (var handler in _subscribers[type])
            {
                ((Action<TEvent>)handler)(@event);
            }
        }

        Console.WriteLine($"[EVENT FIRED] {typeof(TEvent).Name}");
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var type = typeof(TEvent);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(handler);
    }
}

// ======================================================================
// BOOKING SAGA (STATEFUL WORKFLOW + COMPENSATION)
// ======================================================================

public class BookingSaga
{
    private readonly EventDispatcher _dispatcher;
    private bool _flightReserved;
    private bool _hotelBooked;
    private bool _paymentProcessed;

    public BookingSaga(EventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;

        _dispatcher.Subscribe<FlightReservedEvent>(OnFlightReserved);
        _dispatcher.Subscribe<HotelBookedEvent>(OnHotelBooked);
        _dispatcher.Subscribe<PaymentProcessedEvent>(OnPaymentProcessed);

        // Compensation log
        _dispatcher.Subscribe<RefundPaymentEvent>(e =>
            Console.WriteLine("COMPENSATION: Payment refunded."));
        _dispatcher.Subscribe<CancelHotelEvent>(e =>
            Console.WriteLine("COMPENSATION: Hotel booking canceled."));
        _dispatcher.Subscribe<CancelFlightEvent>(e =>
            Console.WriteLine("COMPENSATION: Flight reservation canceled."));
    }

    // ---------------- START SAGA ----------------

    public void StartBooking(Guid bookingId)
    {
        Console.WriteLine("STEP 1: Reserve Flight");
        _dispatcher.Publish(new FlightReservedEvent(bookingId));
    }

    // ---------------- FORWARD ACTIONS ----------------

    private void OnFlightReserved(FlightReservedEvent evt)
    {
        _flightReserved = true;

        Console.WriteLine("STEP 2: Book Hotel");
        _dispatcher.Publish(new HotelBookedEvent(evt.BookingId));
    }

    private void OnHotelBooked(HotelBookedEvent evt)
    {
        _hotelBooked = true;

        Console.WriteLine("STEP 3: Process Payment");
        _dispatcher.Publish(new PaymentProcessedEvent(evt.BookingId));
    }

    private void OnPaymentProcessed(PaymentProcessedEvent evt)
    {
        _paymentProcessed = true;
        Console.WriteLine("🎉 BOOKING COMPLETED SUCCESSFULLY!");
    }

    // ---------------- COMPENSATION LOGIC ----------------

    public void HandleFailure(Guid bookingId)
    {
        Console.WriteLine("!!! FAILURE DETECTED – RUNNING COMPENSATION !!!");

        if (_paymentProcessed)
            _dispatcher.Publish(new RefundPaymentEvent(bookingId));

        if (_hotelBooked)
            _dispatcher.Publish(new CancelHotelEvent(bookingId));

        if (_flightReserved)
            _dispatcher.Publish(new CancelFlightEvent(bookingId));
    }
}

/*
     !=== START BOOKING ===
    STEP 1: Reserve Flight
    STEP 2: Book Hotel
    STEP 3: Process Payment
    ?? BOOKING COMPLETED SUCCESSFULLY!
    [EVENT FIRED] PaymentProcessedEvent
    [EVENT FIRED] HotelBookedEvent
    [EVENT FIRED] FlightReservedEvent

    === SIMULATE FAILURE ===
    !!! FAILURE DETECTED - RUNNING COMPENSATION !!!
    COMPENSATION: Payment refunded.
    [EVENT FIRED] RefundPaymentEvent
    COMPENSATION: Hotel booking canceled.
    [EVENT FIRED] CancelHotelEvent
    COMPENSATION: Flight reservation canceled.
    [EVENT FIRED] CancelFlightEvent
 */

/*
* 1.Event Classes: Define success events for each step (FlightReservedEvent, HotelBookedEvent, PaymentProcessedEvent) and compensation events (CancelFlightEvent, CancelHotelEvent, RefundPaymentEvent).

* 2.EventDispatcher: Manages asynchronous messaging to publish events and subscribe handlers.

* 3.BookingSaga: Coordinates the booking process, with methods to start booking, handle success events, and manage compensations if a failure occurs.
 */