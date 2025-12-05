public class Program
{
    static void Main()
    {
        Console.WriteLine("===== PAYMENT PROCESSOR SYSTEM =====\n");

        PaymentProcessor processor = new PaymentProcessor();

        IPaymentMethod creditCard = new CreditCardPayment();
        IPaymentMethod paypal = new PayPalPayment();
        IPaymentMethod bankTransfer = new BankTransferPayment();

        Console.WriteLine(">>> Trying Credit Card Payment (Amount: 800)");
        processor.MakePayment(800, creditCard);

        Console.WriteLine("\n>>> Trying Credit Card Payment (Amount: 1500)");
        processor.MakePayment(1500, creditCard);

        Console.WriteLine("\n>>> Trying PayPal Payment (Amount: 2000)");
        processor.MakePayment(2000, paypal);

        Console.WriteLine("\n>>> Trying Bank Transfer (Amount: 50)");
        processor.MakePayment(50, bankTransfer);

        Console.WriteLine("\n>>> Trying Bank Transfer (Amount: 3000)");
        processor.MakePayment(3000, bankTransfer);

        Console.WriteLine("\n===== END =====");

        Console.ReadKey();
    }
}

public interface IPaymentMethod
{
    bool ProcessPayment(double amount);
}

public class CreditCardPayment : IPaymentMethod
{
    public bool ProcessPayment(double amount)
    {
        return amount < 1000;
    }
}

public class PayPalPayment : IPaymentMethod
{
    public bool ProcessPayment(double amount)
    {
        return true;
    }
}

public class BankTransferPayment : IPaymentMethod
{
    public bool ProcessPayment(double amount)
    {
        return amount > 100 && amount < 5000;
    }
}

public class PaymentProcessor
{
    public bool MakePayment(double amount, IPaymentMethod paymentMethod)
    {
        bool isSuccess = paymentMethod.ProcessPayment(amount);
        Console.WriteLine(isSuccess ? "Payment successful" : "Payment failed");
        return isSuccess;
    }
}


/*
 * 1.DocumentGenerator Class:

    Defines the GenerateDocument template method that executes the workflow in a specific sequence: header, body, footer.

    Uses protected abstract methods for GenerateHeader, GenerateBody, and GenerateFooter, enforcing that subclasses provide their own implementations.

* 2.ReportGenerator and InvoiceGenerator Classes:

    Each class provides a unique implementation for GenerateHeader, GenerateBody, and GenerateFooter.

    Calling GenerateDocument on each subclass follows the same workflow but with different content.
 */