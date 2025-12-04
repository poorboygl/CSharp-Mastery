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

    // Static field to track total accounts
    private static int TotalAccounts = 0;

    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        _balance = 0;
        TotalAccounts++; // Increment total accounts on each new instance
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

    // Static method to get total accounts
    public static int GetTotalAccounts()
    {
        return TotalAccounts;
    }

    // Static method to reset total accounts (for testing purposes only)
    public static void ResetTotalAccounts()
    {
        TotalAccounts = 0;
    }
}

/*
* 1.Static Field:

    private static int TotalAccounts = 0; declares a static field to track the number of accounts.

    The initial value is set to 0 and will increase each time an account is created.

* 2.Incrementing in Constructor:

    TotalAccounts++ in the constructor increments the counter each time a new instance is created.

* 3.Static Method for Access:

    GetTotalAccounts is a static method that returns the current value of TotalAccounts.
 */