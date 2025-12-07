// ======================================================================
// PROGRAM ENTRY – EVENT SOURCING DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== EVENT SOURCING ACCOUNT DEMO ===\n");

        var account = new Account();
        var handler = new AccountCommandHandler(account);

        // Deposit
        handler.Handle(new DepositCommand(100));
        handler.Handle(new DepositCommand(50));

        // Withdraw
        handler.Handle(new WithdrawCommand(30));

        Console.WriteLine($"\nCurrent Balance: {account.Balance}");

        // Show event history
        Console.WriteLine("\n=== EVENT HISTORY ===");
        foreach (var ev in account.GetEventHistory())
        {
            Console.WriteLine($"- {ev.GetType().Name}");
        }

        // Replay events into new account
        var newAccount = new Account();
        newAccount.ReplayEvents(account.GetEventHistory());

        Console.WriteLine($"\nBalance after replaying events: {newAccount.Balance}");

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// COMMANDS & EVENTS (MARKER INTERFACES)
// ======================================================================
public interface ICommand { }
public interface IEvent { }


// ======================================================================
// COMMAND DEFINITIONS
// ======================================================================
public class DepositCommand : ICommand
{
    public decimal Amount { get; }

    public DepositCommand(decimal amount)
    {
        Amount = amount;
    }
}

public class WithdrawCommand : ICommand
{
    public decimal Amount { get; }

    public WithdrawCommand(decimal amount)
    {
        Amount = amount;
    }
}


// ======================================================================
// EVENT DEFINITIONS
// ======================================================================
public class DepositEvent : IEvent
{
    public decimal Amount { get; }

    public DepositEvent(decimal amount)
    {
        Amount = amount;
    }
}

public class WithdrawEvent : IEvent
{
    public decimal Amount { get; }

    public WithdrawEvent(decimal amount)
    {
        Amount = amount;
    }
}


// ======================================================================
// ACCOUNT AGGREGATE (EVENT SOURCING ROOT)
// ======================================================================
public class Account
{
    private readonly List<IEvent> _events = new();
    public decimal Balance { get; private set; } = 0;

    public void ApplyEvent(IEvent @event)
    {
        switch (@event)
        {
            case DepositEvent deposit:
                Balance += deposit.Amount;
                break;

            case WithdrawEvent withdraw:
                Balance -= withdraw.Amount;
                break;
        }

        _events.Add(@event);
    }

    public void ReplayEvents(IEnumerable<IEvent> events)
    {
        Balance = 0;
        foreach (var @event in events)
        {
            ApplyEvent(@event);
        }
    }

    public IReadOnlyList<IEvent> GetEventHistory() =>
        _events.AsReadOnly();
}


// ======================================================================
// COMMAND HANDLER
// ======================================================================
public class AccountCommandHandler
{
    private readonly Account _account;

    public AccountCommandHandler(Account account)
    {
        _account = account;
    }

    public void Handle(DepositCommand command)
    {
        var depositEvent = new DepositEvent(command.Amount);
        _account.ApplyEvent(depositEvent);
    }

    public void Handle(WithdrawCommand command)
    {
        if (_account.Balance < command.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        var withdrawEvent = new WithdrawEvent(command.Amount);
        _account.ApplyEvent(withdrawEvent);
    }
}

/*
 === EVENT SOURCING ACCOUNT DEMO ===


Current Balance: 120

=== EVENT HISTORY ===
- DepositEvent
- DepositEvent
- WithdrawEvent

Balance after replaying events: 120

=== DONE ===

 */

/*
* 1.Interfaces (ICommand, IEvent): Marker interfaces to distinguish between commands and events.

* 2.Command Classes: DepositCommand and WithdrawCommand encapsulate requests to modify the account’s state.

* 3.Event Classes: DepositEvent and WithdrawEvent represent events that modify the account’s balance.
 */