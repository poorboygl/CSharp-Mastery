public class Program
{
    static void Main()
    {
        var person = new Person
        {
            Name = "Alice",
            Age = 28
        };

        var student = new Student
        {
            Name = "Bob",
            Age = 21,
            StudentId = "SV12345"
        };

        Console.WriteLine("=== Person Introduction ===");
        Console.WriteLine(person.Introduce());
        Console.WriteLine();

        Console.WriteLine("=== Student Introduction ===");
        Console.WriteLine(student.Introduce());
        Console.WriteLine();

        Console.ReadKey();
    }
}

public class Person
{
    public required string Name { get; set; }
    public int Age { get; set; }

    public virtual string Introduce()
    {
        return $"Hello, my name is {Name} and I am {Age} years old.";
    }
}

public class Student : Person
{
    public required string StudentId { get; set; }

    public override string Introduce()
    {
        return $"Hello, my name is {Name}, I am {Age} years old, and my student ID is {StudentId}.";
    }
}

/*
 * 1.Inheritance Syntax:

    public class Student : Person indicates that Student is a subclass of Person.

* 2.Adding the StudentId Property:

    public string StudentId { get; set; } adds a new property to Student.

* 3.Overriding Introduce Method:

    The Introduce method in Student is marked with public override.

    It provides a new implementation that includes the StudentId.

    Uses string interpolation to construct the message
 
 */