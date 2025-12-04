public class Program
{
    static void Main()
    {
        Console.WriteLine("=== BANK ACCOUNT SYSTEM ===\n");

        var account = new BankAccount("ACC12345");

        Console.WriteLine($"Account Number: {account.AccountNumber}");

        Console.WriteLine("\n--> Deposit 500");
        account.Deposit(500);
        Console.WriteLine($"Current Balance: {account.Balance}");

        Console.WriteLine("\n--> Withdraw 200");
        account.Withdraw(200);
        Console.WriteLine($"Current Balance: {account.Balance}");

        Console.WriteLine("\n--> Trying to withdraw 500 (should fail)");
        try
        {
            account.Withdraw(500);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nFinal Balance: " + account.Balance);

        Console.ReadKey();
    }
}
public class BankAccount
{
    private double _balance;
    public string AccountNumber { get; }
    public double Balance => _balance;

    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        _balance = 0;
    }

    public void Deposit(double amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Deposit amount must be positive.");
        }

        _balance += amount;
    }

    public void Withdraw(double amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive.");
        }
        if (amount > _balance)
        {
            throw new InvalidOperationException("Insufficient balance.");
        }

        _balance -= amount;
    }
}

/*
* 1.Private Field for Balance:

    _balance is private to prevent direct modification outside the class.

* 2.Read-Only Properties:

    AccountNumber has only a getter, making it immutable once set.

    Balance is read-only and provides access to _balance.

* 3.Controlled Balance Modification:

    Deposit method checks for negative deposits and throws an exception if needed.

    Withdraw method checks for negative amounts and insufficient balance before deducting.
 */
