public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Bank Account Demo ===\n");

        var account = new BankAccount
        {
            AccountNumber = "123-456-789"
        };

        Console.WriteLine($"Account Created: {account.AccountNumber}");
        Console.WriteLine($"Starting Balance: {account.Balance}\n");

        try
        {
            Console.WriteLine("Depositing 500...");
            account.Deposit(500);
            Console.WriteLine($"New Balance: {account.Balance}\n");

            Console.WriteLine("Withdrawing 200...");
            account.Withdraw(200);
            Console.WriteLine($"New Balance: {account.Balance}\n");

            Console.WriteLine("Attempting to withdraw 1000...");
            account.Withdraw(1000); // sẽ gây lỗi InsufficientFundsException
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"[Invalid Amount Error] {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"[Insufficient Funds Error] {ex.Message}");
        }

        Console.WriteLine("\n=== End of Demo ===");

        Console.ReadKey();
    }
}

public class BankAccount
{
    public required string AccountNumber { get; set; }
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException("Amount must be greater than zero.");
        }

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException("Amount must be greater than zero.");
        }

        if (Balance < amount)
        {
            throw new InsufficientFundsException("Insufficient funds for this withdrawal.");
        }

        Balance -= amount;
    }
}

public class InvalidAmountException : Exception
{
    public InvalidAmountException(string message) : base(message)
    {
    }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message)
    {
    }
}

/*
 === Bank Account Demo ===

Account Created: 123-456-789
Starting Balance: 0

Depositing 500...
New Balance: 500

Withdrawing 200...
New Balance: 300

Attempting to withdraw 1000...
[Insufficient Funds Error] Insufficient funds for this withdrawal.

=== End of Demo ===

 */

/*
This exercise teaches students how to create and use custom exceptions to enforce business rules and handle specific error scenarios.

* 1.Custom Exceptions:

    InvalidAmountException and InsufficientFundsException provide more context for the error.

* 2.Enforcing Business Rules:

    Deposit and Withdraw enforce rules to prevent invalid or insufficient transactions by throwing specific exceptions.
 */

