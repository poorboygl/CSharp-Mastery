public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Bank Account System Demo ===\n");

        Bank bank = new Bank();

        // Create accounts
        var savings = new SavingsAccount();
        var checking = new CheckingAccount();

        // Deposit money
        savings.Deposit(500);
        checking.Deposit(300);

        // Add to bank
        bank.AddAccount(savings);
        bank.AddAccount(checking);

        // Try withdrawals
        try
        {
            savings.Withdraw(450); // Will throw exception (balance cannot go below 100)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Savings Account Error: {ex.Message}");
        }

        checking.Withdraw(100);

        Console.WriteLine();
        bank.PrintAllBalances();

        Console.ReadKey();
    }
}

public interface IBankAccount
{
    void Deposit(double amount);
    void Withdraw(double amount);
    double GetBalance();
}

public abstract class BankAccount : IBankAccount
{
    protected double _balance;

    public void Deposit(double amount)
    {
        _balance += amount;
    }

    public double GetBalance()
    {
        return _balance;
    }

    public abstract void Withdraw(double amount);
}

public class SavingsAccount : BankAccount
{
    public override void Withdraw(double amount)
    {
        if (_balance - amount < 100)
        {
            throw new InvalidOperationException("Insufficient balance in savings account.");
        }
        _balance -= amount;
    }
}

public class CheckingAccount : BankAccount
{
    public override void Withdraw(double amount)
    {
        if (_balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds in checking account.");
        }
        _balance -= amount;
    }
}

public class Bank
{
    private readonly List<IBankAccount> _accounts = new List<IBankAccount>();

    public void AddAccount(IBankAccount account)
    {
        _accounts.Add(account);
    }

    public void PrintAllBalances()
    {
        foreach (var account in _accounts)
        {
            Console.WriteLine($"Balance: {account.GetBalance():C}");
        }
    }
}

/*
* 1.IBankAccount Interface:

    Provides a contract for account functionality: deposit, withdraw, and get balance.

* 2.BankAccount Class:

    Implements deposit and balance reporting.

    Defines Withdraw as an abstract method to be overridden by derived classes.

* 3.SavingsAccount and CheckingAccount:

    SavingsAccount enforces a minimum balance of 100 for withdrawals.

    CheckingAccount allows withdrawals up to the current balance.

* 4.Bank Class:

    Holds a list of IBankAccount instances, allowing flexibility in adding any account type.

    PrintAllBalances uses polymorphism to call GetBalance on each account.
 */