public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Employee Payroll Demo ===");

        List<Employee> employees = new List<Employee>
        {
            new FullTimeEmployee("Alice", 3000),
            new PartTimeEmployee("Bob", 15, 80),
            new Manager("Charlie", 5000, 1200)
        };

        PayrollProcessor processor = new PayrollProcessor();
        double totalMonthlySalary = processor.CalculateTotalMonthlySalaries(employees);

        foreach (var emp in employees)
        {
            Console.WriteLine($"{emp.Name} earns: {emp.CalculateMonthlySalary()}");
        }

        Console.WriteLine("------------------------------");
        Console.WriteLine("Total monthly salary payout: " + totalMonthlySalary);

        Console.ReadKey();
    }
}

public abstract class Employee
{
    public string Name { get; set; }
    public double BaseSalary { get; set; }

    public Employee(string name, double baseSalary)
    {
        Name = name;
        BaseSalary = baseSalary;
    }

    public abstract double CalculateMonthlySalary();
}

public class FullTimeEmployee : Employee
{
    public FullTimeEmployee(string name, double baseSalary) : base(name, baseSalary) { }

    public override double CalculateMonthlySalary()
    {
        return BaseSalary;
    }
}

public class PartTimeEmployee : Employee
{
    public double HourlyRate { get; set; }
    public int HoursWorked { get; set; }

    public PartTimeEmployee(string name, double hourlyRate, int hoursWorked)
        : base(name, hourlyRate * hoursWorked)
    {
        HourlyRate = hourlyRate;
        HoursWorked = hoursWorked;
    }

    public override double CalculateMonthlySalary()
    {
        return HourlyRate * HoursWorked;
    }
}

public class Manager : Employee
{
    public double Bonus { get; set; }

    public Manager(string name, double baseSalary, double bonus) : base(name, baseSalary)
    {
        Bonus = bonus;
    }

    public override double CalculateMonthlySalary()
    {
        return BaseSalary + Bonus;
    }
}

public class PayrollProcessor
{
    public double CalculateTotalMonthlySalaries(List<Employee> employees)
    {
        double total = 0;
        foreach (var employee in employees)
        {
            total += employee.CalculateMonthlySalary();
        }
        return total;
    }
}

/*
* 1.Employee Class:

Employee is an abstract class that requires derived classes to implement CalculateMonthlySalary.

* 2.FullTimeEmployee:

CalculateMonthlySalary returns BaseSalary directly.

* 3.PartTimeEmployee:

HourlyRate and HoursWorked are used to calculate the monthly salary.

* 4.Manager:

CalculateMonthlySalary returns the sum of BaseSalary and Bonus.

* 5.PayrollProcessor:

CalculateTotalMonthlySalaries iterates through each Employee, calling CalculateMonthlySalary on each instance.
 
 */