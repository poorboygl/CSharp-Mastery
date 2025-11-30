public class Program
{
    static void Main()
    {
        var converter = new DayConverter();

        int inputDay = 3;

        Console.WriteLine("=== ConvertToWeekday Result ===");
        Console.WriteLine("Input Number: " + inputDay);

        string weekday = converter.ConvertToWeekday(inputDay);

        Console.WriteLine("Converted Weekday: " + weekday);

        Console.ReadKey();
    }
}

public enum DayOfWeek
{
    Monday = 1,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public class DayConverter
{
    public string ConvertToWeekday(int dayNumber)
    {
        switch (dayNumber)
        {
            case (int)DayOfWeek.Monday:
                return "Monday";
            case (int)DayOfWeek.Tuesday:
                return "Tuesday";
            case (int)DayOfWeek.Wednesday:
                return "Wednesday";
            case (int)DayOfWeek.Thursday:
                return "Thursday";
            case (int)DayOfWeek.Friday:
                return "Friday";
            case (int)DayOfWeek.Saturday:
                return "Saturday";
            case (int)DayOfWeek.Sunday:
                return "Sunday";
            default:
                return "Invalid day";
        }
    }
}

