public class Program
{
    static void Main()
    {
        var customers = new List<Customer>
        {
            new() { Name = "Alice", City = "New York", Orders = [200, 150, 50] },
            new() { Name = "Bob", City = "Chicago", Orders = [500, 20] },
            new() { Name = "Charlie", City = "Los Angeles", Orders = [100, 200, 300] },
            new() { Name = "Diana", City = "Houston", Orders = [50] }
        };

        Console.WriteLine("=== Top Spending Customers Result ===");

        int topN = 3;
        var topCustomers = CustomerAnalyzer.GetTopSpendingCustomers(customers, topN);

        Console.WriteLine($"Top {topN} Customers by Total Spending:");
        foreach (var c in topCustomers)
        {
            Console.WriteLine($"{c.Name} ({c.City}) - Total Spent: {c.Orders.Sum()}");
        }

        Console.ReadKey();
    }
}
public class Customer
{
    public required string Name { get; set; }
    public required string City { get; set; }
    public required List<decimal> Orders { get; set; } 
}

public class CustomerAnalyzer
{
    public static List<Customer> GetTopSpendingCustomers(List<Customer> customers, int n)
    {
        return customers
            .OrderByDescending(c => c.Orders.Sum())
            .Take(n)
            .ToList();
    }
}

/*
* 1.Calculate Total Spending:

Use Select to create an anonymous type containing each customer’s name and total order amount.

* 2.Order and Take Top Customers:

Order customers by total spending in descending order, then use Take to get the top n.

* 3.Return Result:

Return the top customers in the original Customer object form.
 
 */