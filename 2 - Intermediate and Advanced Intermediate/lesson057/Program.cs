public class Program
{
    static void Main()
    {
        Console.WriteLine("===== PAYMENT STRATEGY DEMO =====\n");

        // Demo 1: Credit Card
        Console.WriteLine("--- Using Credit Card Payment ---");
        var creditCardProcessor = new PaymentProcessor(new CreditCardPayment());
        creditCardProcessor.Process(150.75m);

        // Demo 2: PayPal
        Console.WriteLine("\n--- Using PayPal Payment ---");
        var paypalProcessor = new PaymentProcessor(new PayPalPayment());
        paypalProcessor.Process(89.99m);

        // Demo 3: Bitcoin
        Console.WriteLine("\n--- Using Bitcoin Payment ---");
        var bitcoinProcessor = new PaymentProcessor(new BitcoinPayment());
        bitcoinProcessor.Process(0.005m);

        Console.WriteLine("\n===== END =====");

        Console.ReadKey();
    }
}


public interface IPaymentStrategy
    {
        void ProcessPayment(decimal amount);
    }

    public class CreditCardPayment : IPaymentStrategy
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount}");
        }
    }

    public class PayPalPayment : IPaymentStrategy
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of ${amount}");
        }
    }

    public class BitcoinPayment : IPaymentStrategy
    {
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing Bitcoin payment of ${amount}");
        }
    }

    public class PaymentProcessor
    {
        private readonly IPaymentStrategy _paymentStrategy;

        public PaymentProcessor(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void Process(decimal amount)
        {
            _paymentStrategy.ProcessPayment(amount);
        }
    }

/*
* 1.IPaymentStrategy Interface:

    Defines the ProcessPayment method for various payment strategies.

* 2.Payment Strategy Implementations:

    CreditCardPayment, PayPalPayment, and BitcoinPayment each implement ProcessPayment, displaying the payment method and amount.

* 3.PaymentProcessor Class:

    Accepts any IPaymentStrategy implementation through its constructor and delegates the Process method to the injected strategy
 */


