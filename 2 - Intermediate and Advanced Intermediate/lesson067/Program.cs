using System.Reflection;

public class Program
{
    static void Main()
    {
        Console.WriteLine("===== VALIDATION ATTRIBUTE DEMO =====\n");

        var person = new Person
        {
            Name = "John Doe",
            Age = 25
        };

        try
        {
            Validator.Validate(person);
            Console.WriteLine("Person is valid!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }

        Console.WriteLine("\n===== END =====");
        Console.ReadKey();
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }

    public RangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}

public class Validator
{
    public static void Validate(object obj)
    {
        var type = obj.GetType();
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(obj);

            if (Attribute.IsDefined(property, typeof(RequiredAttribute)))
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    throw new InvalidOperationException($"{property.Name} is required.");
                }
            }

            var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute != null && value is int intValue)
            {
                if (intValue < rangeAttribute.Min || intValue > rangeAttribute.Max)
                {
                    throw new InvalidOperationException($"{property.Name} must be between {rangeAttribute.Min} and {rangeAttribute.Max}.");
                }
            }
        }
    }
}

public class Person
{
    [Required]
    public required string Name { get; set; }

    [Range(0, 120)]
    public int Age { get; set; }
}


/*
* 1.RequiredAttribute and RangeAttribute:

    RequiredAttribute marks properties that must not be null or empty.

    RangeAttribute specifies a range of acceptable values using Min and Max.

* 2.Validator Class:

    Validate uses reflection to inspect properties of the provided object.

    It checks for RequiredAttribute and ensures that properties with this attribute are non-null and non-empty.

    It checks for RangeAttribute and ensures that numeric values are within the specified range.
 
 */