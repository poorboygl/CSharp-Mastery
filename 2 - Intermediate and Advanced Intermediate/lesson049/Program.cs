public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Student Grade Analyzer ===");

        var student = new Student
        {
            Name = "Alice",
            MidtermGrade = 85,
            FinalGrade = 92,
            AssignmentGrade = 78
        };

        Console.WriteLine($"Student: {student.Name}");

        double? average = student.CalculateAverageGrade();
        Console.WriteLine($"Average Grade: {(average.HasValue ? average.Value.ToString("F2") : "N/A")}");

        Console.WriteLine($"Letter Grade: {student.GetLetterGrade()}");

        Console.ReadKey();
    }
}
public class Student
{
    public required string Name { get; set; }
    public double? MidtermGrade { get; set; }
    public double? FinalGrade { get; set; }
    public double? AssignmentGrade { get; set; }

    public double? CalculateAverageGrade()
    {
        var grades = new List<double?> { MidtermGrade, FinalGrade, AssignmentGrade }
                     .Where(g => g.HasValue)
                     .Select(g => g.Value)
                     .ToList();

        return grades.Any() ? (double?)grades.Average() : null;
    }

    public string GetLetterGrade()
    {
        var average = CalculateAverageGrade();
        if (average == null)
        {
            return "No Grade";
        }

        if (average >= 90) return "A";
        if (average >= 80) return "B";
        if (average >= 70) return "C";
        if (average >= 60) return "D";
        return "F";
    }
}

/*
 === Student Grade Analyzer ===
Student: Alice
Average Grade: 85.00
Letter Grade: B
 
 */

/*
* 1.Calculating Average:

CalculateAverageGrade uses Where to filter out null grades, then calculates the average of the remaining values.

* 2.Determining Letter Grade:

GetLetterGrade uses a series of conditional checks to return the appropriate letter grade or "No Grade" if all grades are null.
 */