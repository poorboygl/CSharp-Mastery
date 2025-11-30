public class Program
{
    static void Main()
    {
        var calculator = new DateCalculator();

        DateTime dob = new DateTime(1995, 10, 15);

        Console.WriteLine("=== CalculateAge Result ===");
        Console.WriteLine("Date of Birth: " + dob.ToString("yyyy-MM-dd"));

        int age = calculator.CalculateAge(dob);

        Console.WriteLine("Calculated Age: " + age);

        Console.ReadKey();
    }
}
public class DateCalculator
{
    public int CalculateAge(DateTime dateOfBirth)
    {
        int age = DateTime.Now.Year - dateOfBirth.Year;

        // Check if birthday has occurred this year
        if (DateTime.Now < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age;
    }
}

/*
* 1.Calculate Initial Age:

DateTime.Now.Year - dateOfBirth.Year gives the preliminary age.

* 2.Adjust for Upcoming Birthday:

dateOfBirth.AddYears(age) represents the birthday this year. If today is before this date, the birthday has not yet occurred, so we subtract one from the age.
 */