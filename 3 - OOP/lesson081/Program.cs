public class Program
{
    static void Main()
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 30
        };

        Console.WriteLine("=== Person Introduction ===");
        Console.WriteLine(person.Introduce());

        Console.ReadKey();
    }
}
public class Person
{
    public required string Name { get; set; }
    public int Age { get; set; }

    public string Introduce()
    {
        return $"Hello, my name is {Name} and I am {Age} years old.";
    }
}

/*

* 1.Properties:

public string Name { get; set; } defines a public property Name with automatic getters and setters.

public int Age { get; set; } defines a public property Age similarly.

* 2.Introduce Method:

public string Introduce() declares a method that can be called on a Person object.

The method returns a string that includes the Name and Age property values using string interpolation.
 */