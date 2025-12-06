public class Program
{
    static async Task Main()
    {
        Console.Title = "CQRS + Event Sourcing Demo";
        Console.WriteLine("=== CQRS + EVENT SOURCING BANK ACCOUNT DEMO ===\n");

        var accountId = Guid.NewGuid();
        var eventStore = new EventStore();
        var queryHandler = new GetBalanceQueryHandler(eventStore);

        var account = new BankAccount(accountId);

        Console.WriteLine("Performing deposit of 100...");
        account.Deposit(100);

        Console.WriteLine("Performing withdrawal of 40...");
        account.Withdraw(40);

        // Save uncommitted events to the store
        Console.WriteLine("\nSaving events to event store...");
        foreach (var evt in account.GetUncommittedEvents())
        {
            eventStore.Save(evt);
        }

        // Query balance
        Console.WriteLine("\nQuerying balance...");
        decimal balance = await queryHandler.HandleAsync(new GetBalanceQuery(accountId));

        Console.WriteLine($"\nFinal Balance: {balance}");

        Console.ReadKey();
    }
}


public interface ICommand { }

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface IEvent { }

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}

public interface IQuery<TResponse> { }

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query);
}

public class DepositCommand : ICommand
{
    public Guid AccountId { get; }
    public decimal Amount { get; }

    public DepositCommand(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}

public class WithdrawCommand : ICommand
{
    public Guid AccountId { get; }
    public decimal Amount { get; }

    public WithdrawCommand(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}

public class DepositEvent : IEvent
{
    public Guid AccountId { get; }
    public decimal Amount { get; }

    public DepositEvent(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}

public class WithdrawEvent : IEvent
{
    public Guid AccountId { get; }
    public decimal Amount { get; }

    public WithdrawEvent(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}

public class BankAccount
{
    public Guid Id { get; }
    private readonly List<IEvent> _events = new List<IEvent>();

    public BankAccount(Guid id)
    {
        Id = id;
    }

    public void Deposit(decimal amount)
    {
        _events.Add(new DepositEvent(Id, amount));
    }

    public void Withdraw(decimal amount)
    {
        _events.Add(new WithdrawEvent(Id, amount));
    }

    public IEnumerable<IEvent> GetUncommittedEvents() => _events;
}

public class EventStore
{
    private readonly List<IEvent> _storedEvents = new List<IEvent>();

    public void Save(IEvent @event)
    {
        _storedEvents.Add(@event);
    }

    public IEnumerable<IEvent> GetEvents(Guid accountId)
    {
        foreach (var e in _storedEvents)
        {
            if ((e as dynamic).AccountId == accountId)
                yield return e;
        }
    }
}

public class GetBalanceQuery : IQuery<decimal>
{
    public Guid AccountId { get; }

    public GetBalanceQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}

public class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, decimal>
{
    private readonly EventStore _eventStore;

    public GetBalanceQueryHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<decimal> HandleAsync(GetBalanceQuery query)
    {
        decimal balance = 0;
        foreach (var e in _eventStore.GetEvents(query.AccountId))
        {
            switch (e)
            {
                case DepositEvent deposit:
                    balance += deposit.Amount;
                    break;
                case WithdrawEvent withdraw:
                    balance -= withdraw.Amount;
                    break;
            }
        }
        return balance;
    }
}

/*
 === CQRS + EVENT SOURCING BANK ACCOUNT DEMO ===

Performing deposit of 100...
Performing withdrawal of 40...

Saving events to event store...

Querying balance...

Final Balance: 60
 */

/*
* 1.Command and Query Interfaces:

    ICommand, ICommandHandler, IQuery, and IQueryHandler define the structure for handling commands and queries in a decoupled way.

* 2.Bank Account and Events:
    
    DepositCommand and WithdrawCommand generate events that are stored in EventStore.

    GetBalanceQueryHandler calculates the balance by replaying events in EventS
 */