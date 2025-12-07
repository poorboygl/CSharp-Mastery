// ======================================================================
// EVENT DISPATCHER + EVENT SOURCING SYSTEM - DEMO
// ======================================================================

public class Program
{
    static void Main()
    {
        // ==================================================================
        // DEMO CHẠY THỬ
        // ==================================================================

        var dispatcher = new EventDispatcher();
        var accountService = new AccountService(dispatcher);
        var notificationService = new NotificationService(dispatcher);

        // ---- Đăng ký subscribe ----
        dispatcher.Subscribe<TransactionProcessedEvent>(notificationService.OnTransactionProcessed);

        // ---- Tạo tài khoản ----
        var accountId = Guid.NewGuid();

        // ---- Giao dịch ----
        accountService.ProcessTransaction(accountId, 1000);
        accountService.ProcessTransaction(accountId, 500);
        accountService.ProcessTransaction(accountId, -200);

        // ---- In kết quả ----
        Console.WriteLine("===== CURRENT ACCOUNT BALANCE =====");
        Console.WriteLine($"Account: {accountId}");
        Console.WriteLine($"Balance: {accountService.GetBalance(accountId)}");

        Console.WriteLine();
        Console.WriteLine("===== PUBLISHED EVENTS =====");
        foreach (var evt in dispatcher.GetPublishedEvents())
        {
            Console.WriteLine($"Event: {evt.GetType().Name}");
        }

        Console.WriteLine();
        Console.WriteLine("===== REPLAY DEMO =====");
        var replayAccountService = new AccountService(new EventDispatcher());
        replayAccountService.Replay(dispatcher.GetPublishedEvents());

        Console.WriteLine($"Balance After Replay: {replayAccountService.GetBalance(accountId)}");

        Console.ReadKey();
    }
}

// ======================================================================
// EVENT INTERFACE + EVENT DEFINITIONS
// ======================================================================

public interface IEvent { }

public class TransactionProcessedEvent : IEvent
{
    public Guid AccountId { get; }
    public decimal Amount { get; }

    public TransactionProcessedEvent(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}

public class AccountBalanceUpdatedEvent : IEvent
{
    public Guid AccountId { get; }
    public decimal NewBalance { get; }

    public AccountBalanceUpdatedEvent(Guid accountId, decimal newBalance)
    {
        AccountId = accountId;
        NewBalance = newBalance;
    }
}

public class NotificationSentEvent : IEvent
{
    public Guid AccountId { get; }
    public string Message { get; }

    public NotificationSentEvent(Guid accountId, string message)
    {
        AccountId = accountId;
        Message = message;
    }
}

// ======================================================================
// EVENT DISPATCHER (PUB-SUB + EVENT STORE)
// ======================================================================

public class EventDispatcher
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly List<IEvent> _eventHistory = new(); // Track published events

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var handler in _subscribers[eventType])
            {
                ((Action<TEvent>)handler)(@event);
            }
        }

        _eventHistory.Add(@event); // Save to event history
    }

    public IEnumerable<IEvent> GetPublishedEvents() => _eventHistory;

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var type = typeof(TEvent);
        if (!_subscribers.ContainsKey(type))
        {
            _subscribers[type] = new List<Delegate>();
        }
        _subscribers[type].Add(handler);
    }
}

// ======================================================================
// ACCOUNT SERVICE (WRITE MODEL)
// ======================================================================

public class AccountService
{
    private readonly EventDispatcher _eventDispatcher;
    private readonly Dictionary<Guid, decimal> _accountBalances = new();

    public AccountService(EventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    public void ProcessTransaction(Guid accountId, decimal amount)
    {
        if (!_accountBalances.ContainsKey(accountId))
        {
            _accountBalances[accountId] = 0;
        }

        _accountBalances[accountId] += amount;

        _eventDispatcher.Publish(new AccountBalanceUpdatedEvent(accountId, _accountBalances[accountId]));
        _eventDispatcher.Publish(new TransactionProcessedEvent(accountId, amount));
    }

    public decimal GetBalance(Guid accountId) =>
        _accountBalances.TryGetValue(accountId, out var b) ? b : 0;

    // Replay để dựng lại state từ event sourcing
    public void Replay(IEnumerable<IEvent> events)
    {
        foreach (var evt in events)
        {
            if (evt is AccountBalanceUpdatedEvent balanceEvent)
            {
                _accountBalances[balanceEvent.AccountId] = balanceEvent.NewBalance;
            }
        }
    }
}

// ======================================================================
// NOTIFICATION SERVICE (READ MODEL)
// ======================================================================

public class NotificationService
{
    private readonly EventDispatcher _eventDispatcher;
    private readonly HashSet<Guid> _notifiedTransactions = new();

    public NotificationService(EventDispatcher dispatcher)
    {
        _eventDispatcher = dispatcher;
    }

    public void OnTransactionProcessed(TransactionProcessedEvent @event)
    {
        if (!_notifiedTransactions.Contains(@event.AccountId))
        {
            var msg = $"Transaction {@event.Amount} processed for account {@event.AccountId}";
            _eventDispatcher.Publish(new NotificationSentEvent(@event.AccountId, msg));
            _notifiedTransactions.Add(@event.AccountId);
        }
    }

    public void Replay(IEnumerable<IEvent> events)
    {
        foreach (var evt in events)
        {
            if (evt is TransactionProcessedEvent t)
            {
                OnTransactionProcessed(t);
            }
        }
    }
}

/*
* 1.Event Interfaces and Classes: IEvent is the marker interface for events, while TransactionProcessedEvent, AccountBalanceUpdatedEvent, and NotificationSentEvent represent significant occurrences.

* 2.EventDispatcher: Manages the asynchronous distribution of events, enabling loose coupling between AccountService and NotificationService.

* 3.AccountService: Processes transactions and updates account balances, publishing events on balance updates and processed transactions.

* 4.NotificationService: Listens to transaction events to send notifications and uses a replay method to resend notifications as needed.
 */