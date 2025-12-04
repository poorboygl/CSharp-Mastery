public class Program
{
    static void Main()
    {
        Console.WriteLine("=== EMPLOYEE SALARY CHECK SYSTEM ===\n");

        var emp = new Employee
        {
            Name = "John Doe",
            Salary = 120000
        };

        Console.WriteLine($"Employee Name: {emp.Name}");
        Console.WriteLine($"Salary: {emp.Salary}");
        Console.WriteLine($"Is High Earner? {emp.IsHighEarner}");

        Console.WriteLine("\n--- Testing invalid salary input ---");
        try
        {
            emp.Salary = -5000;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.ReadKey();
    }
}

public class Employee
{
    public required string Name { get; set; }

    private double _salary;
    public double Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Salary cannot be negative.");
            }
            _salary = value;
        }
    }

    public bool IsHighEarner => Salary >= 100000;
}

/*
* 1.Name Property:

    public string Name { get; set; } is a simple auto-implemented property.

* 2.Salary Property with Validation:

    Salary uses a private field _salary to store the actual value.

The setter includes validation to ensure Salary is non-negative; otherwise, it throws an ArgumentException.

* 3.IsHighEarner Property:

    public bool IsHighEarner => Salary >= 100000; is a read-only property that checks if Salary is above or equal to 100,000.
 */