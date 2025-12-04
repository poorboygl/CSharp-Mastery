public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Parallel Sales Aggregator Demo ===");

        var sales = new List<decimal>
        {
            120.50m, 300.00m, 450.75m, 99.99m, 820.30m
        };

        Console.WriteLine("\nSales data:");
        sales.ForEach(s => Console.WriteLine($"${s}"));

        var aggregator = new SalesAggregator();

        decimal total = aggregator.GetTotalRevenue(sales);
        decimal avg = aggregator.GetAverageRevenue(sales);
        int count = aggregator.GetTransactionCount(sales);

        Console.WriteLine("\nResults:");
        Console.WriteLine($"Total Revenue: ${total}");
        Console.WriteLine($"Average Revenue: ${avg}");
        Console.WriteLine($"Transaction Count: {count}");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class SalesAggregator
{
    public decimal GetTotalRevenue(List<decimal> sales)
    {
        return sales.AsParallel().Sum();
    }

    public decimal GetAverageRevenue(List<decimal> sales)
    {
        return sales.AsParallel().Average();
    }

    public int GetTransactionCount(List<decimal> sales)
    {
        return sales.AsParallel().Count();
    }
}

/*
 * 1.Parallel LINQ (PLINQ):

    .AsParallel() enables parallel processing, allowing each aggregation (sum, average, count) to be distributed across multiple cores.

* 2.Aggregation Methods:

    GetTotalRevenue uses Sum, GetAverageRevenue uses Average, and GetTransactionCount uses Count with PLINQ to process data efficiently.
 */