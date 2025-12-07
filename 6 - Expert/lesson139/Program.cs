using System;
using System.Collections.Generic;

// ======================================================================
// PROGRAM DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== EVENT-DRIVEN BOOKING SYSTEM DEMO ===");

        var eventBus = new EventBus();

        var bookingService = new BookingService(eventBus);
        var paymentService = new PaymentService(eventBus);
        var notificationService = new NotificationService(eventBus);

        Guid bookingId = Guid.NewGuid();

        bookingService.CreateBooking(bookingId);

        Console.WriteLine("\n=== END OF FLOW ===");
        Console.ReadKey();
    }
}

// ======================================================================
// EVENT INTERFACE + EVENTS
// ======================================================================
public interface IEvent { }

public class BookingCreatedEvent : IEvent
{
    public Guid BookingId { get; }
    public BookingCreatedEvent(Guid bookingId) => BookingId = bookingId;
}

public class PaymentProcessedEvent : IEvent
{
    public Guid BookingId { get; }
    public PaymentProcessedEvent(Guid bookingId) => BookingId = bookingId;
}

public class BookingConfirmedEvent : IEvent
{
    public Guid BookingId { get; }
    public BookingConfirmedEvent(Guid bookingId) => BookingId = bookingId;
}

public class PaymentFailedEvent : IEvent
{
    public Guid BookingId { get; }
    public PaymentFailedEvent(Guid bookingId) => BookingId = bookingId;
}

// ======================================================================
// EVENT BUS (IN-MEMORY PUB/SUB)
// ======================================================================
public class EventBus
{
    private readonly Dictionary<Type, List<Action<IEvent>>> _handlers = new();

    public void Publish(IEvent @event)
    {
        var eventType = @event.GetType();
        if (_handlers.ContainsKey(eventType))
        {
            foreach (var handler in _handlers[eventType])
            {
                handler(@event);
            }
        }
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        if (!_handlers.ContainsKey(typeof(TEvent)))
        {
            _handlers[typeof(TEvent)] = new List<Action<IEvent>>();
        }
        _handlers[typeof(TEvent)].Add(e => handler((TEvent)e));
    }
}

// ======================================================================
// BOOKING SERVICE
// ======================================================================
public class BookingService
{
    private readonly EventBus _eventBus;

    public BookingService(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<PaymentFailedEvent>(OnPaymentFailed);
    }

    public void CreateBooking(Guid bookingId)
    {
        Console.WriteLine("[BookingService] Booking created.");
        _eventBus.Publish(new BookingCreatedEvent(bookingId));
    }

    private void OnPaymentFailed(PaymentFailedEvent @event)
    {
        Console.WriteLine($"[BookingService] Booking canceled due to payment failure: {@event.BookingId}");
    }
}

// ======================================================================
// PAYMENT SERVICE
// ======================================================================
public class PaymentService
{
    private readonly EventBus _eventBus;

    public PaymentService(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<BookingCreatedEvent>(OnBookingCreated);
    }

    private void OnBookingCreated(BookingCreatedEvent @event)
    {
        Console.WriteLine("[PaymentService] Processing payment...");

        bool paymentSuccess = new Random().Next(2) == 0;

        if (paymentSuccess)
        {
            Console.WriteLine("[PaymentService] Payment processed.");
            _eventBus.Publish(new PaymentProcessedEvent(@event.BookingId));
        }
        else
        {
            Console.WriteLine("[PaymentService] Payment failed.");
            _eventBus.Publish(new PaymentFailedEvent(@event.BookingId));
        }
    }
}

// ======================================================================
// NOTIFICATION SERVICE
// ======================================================================
public class NotificationService
{
    private readonly EventBus _eventBus;

    public NotificationService(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<PaymentProcessedEvent>(OnPaymentProcessed);
    }

    private void OnPaymentProcessed(PaymentProcessedEvent @event)
    {
        Console.WriteLine("[NotificationService] Sending confirmation notification...");
        _eventBus.Publish(new BookingConfirmedEvent(@event.BookingId));
        Console.WriteLine("[NotificationService] Booking confirmed.");
    }
}

/*
 !=== EVENT-DRIVEN BOOKING SYSTEM DEMO ===
    [BookingService] Booking created.
    [PaymentService] Processing payment...
    [PaymentService] Payment processed.
    [NotificationService] Sending confirmation notification...
    [NotificationService] Booking confirmed.

=== END OF FLOW ===
 */

/*
* 1.EventBus: Manages subscriptions and publishing, allowing each service to listen for specific events.

* 2.BookingService: Initiates the saga by creating a booking and emitting BookingCreatedEvent.

* 3.PaymentService: Listens for BookingCreatedEvent, processes the payment, and emits PaymentProcessedEvent if successful or PaymentFailedEvent if it fails.

* 4.NotificationService: Listens for PaymentProcessedEvent, sends a notification, and emits BookingConfirmedEvent to complete the saga.
 */