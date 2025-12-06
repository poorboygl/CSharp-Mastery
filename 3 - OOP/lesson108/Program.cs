public class Program
{
    static void Main()
    {
        Console.WriteLine("=== ATM State Pattern Demo ===");

        var atm = new ATM();

        Console.WriteLine("\n[Step] Insert card:");
        atm.InsertCard();

        Console.WriteLine("\n[Step] Enter PIN:");
        atm.EnterPin(1234);

        Console.WriteLine("\n[Step] Withdraw Cash:");
        atm.WithdrawCash(500);

        Console.ReadKey();
    }
}
public interface ATMState
{
    void InsertCard();
    void EnterPin(int pin);
    void WithdrawCash(int amount);
}

public class NoCardState : ATMState
{
    private readonly ATM _atm;

    public NoCardState(ATM atm)
    {
        _atm = atm;
    }

    public void InsertCard()
    {
        Console.WriteLine("Card inserted.");
        _atm.SetState(_atm.HasCardState);
    }

    public void EnterPin(int pin)
    {
        Console.WriteLine("Please insert your card.");
    }

    public void WithdrawCash(int amount)
    {
        Console.WriteLine("Please insert your card.");
    }
}

public class HasCardState : ATMState
{
    private readonly ATM _atm;

    public HasCardState(ATM atm)
    {
        _atm = atm;
    }

    public void InsertCard()
    {
        Console.WriteLine("A card is already inserted.");
    }

    public void EnterPin(int pin)
    {
        if (pin == 1234)
        {
            Console.WriteLine("PIN correct.");
            _atm.SetState(_atm.PinVerifiedState);
        }
        else
        {
            Console.WriteLine("Incorrect PIN.");
        }
    }

    public void WithdrawCash(int amount)
    {
        Console.WriteLine("Please enter your PIN first.");
    }
}

public class PinVerifiedState : ATMState
{
    private readonly ATM _atm;

    public PinVerifiedState(ATM atm)
    {
        _atm = atm;
    }

    public void InsertCard()
    {
        Console.WriteLine("A card is already inserted.");
    }

    public void EnterPin(int pin)
    {
        Console.WriteLine("PIN already entered.");
    }

    public void WithdrawCash(int amount)
    {
        Console.WriteLine($"Dispensing {amount} cash.");
        _atm.SetState(_atm.NoCardState);
    }
}

public class ATM
{
    public ATMState NoCardState { get; private set; }
    public ATMState HasCardState { get; private set; }
    public ATMState PinVerifiedState { get; private set; }

    private ATMState _currentState;

    public ATM()
    {
        NoCardState = new NoCardState(this);
        HasCardState = new HasCardState(this);
        PinVerifiedState = new PinVerifiedState(this);

        _currentState = NoCardState;
    }

    public void SetState(ATMState state)
    {
        _currentState = state;
    }

    public void InsertCard()
    {
        _currentState.InsertCard();
    }

    public void EnterPin(int pin)
    {
        _currentState.EnterPin(pin);
    }

    public void WithdrawCash(int amount)
    {
        _currentState.WithdrawCash(amount);
    }
}

/*
* 1.ATMState Interface:

    Defines methods for InsertCard, EnterPin, and WithdrawCash.

* 2.Concrete States:

    NoCardState: Transitions to HasCardState on InsertCard and prompts for a card on other actions.

    HasCardState: Checks the pin, transitioning to PinVerifiedState if correct and staying in HasCardState otherwise.
    
    PinVerifiedState: Dispenses cash on WithdrawCash and returns to NoCardState afterward.

* 3.ATM Class with State Pattern:

    Holds references to each state, transitioning between them using SetState.

    Delegates actions to _currentState, allowing dynamic behavior changes.
 */