public class Program
{
    static void Main()
    {
        var calculator = new DateDifferenceCalculator();

        DateTime startDate = new DateTime(2025, 1, 1);
        DateTime endDate = new DateTime(2025, 11, 30);

        Console.WriteLine("=== DateDifferenceCalculator Result ===");
        Console.WriteLine("Start Date: " + startDate.ToString("yyyy-MM-dd"));
        Console.WriteLine("End Date: " + endDate.ToString("yyyy-MM-dd"));

        int daysBetween = calculator.CalculateDaysBetween(startDate, endDate);

        Console.WriteLine("Number of Days Between: " + daysBetween);

        Console.ReadKey();
    }
}

public class DateDifferenceCalculator
{
    public int CalculateDaysBetween(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return 0;
        }

        TimeSpan difference = endDate - startDate;
        return difference.Days;
    }
}

/*
* 1.Check if startDate is After endDate:

If startDate is after endDate, return 0 to indicate no valid duration.

* 2.Calculate Days Difference:

endDate - startDate gives a TimeSpan object, and difference.Days returns the number of days
 */