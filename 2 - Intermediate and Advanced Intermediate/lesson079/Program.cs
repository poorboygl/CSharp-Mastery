using System.Reflection;

public class Program
{
    static void Main()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Super Long Product Name That Exceeds The MaxLength Limit!!!!",
            Description = "This is a valid description."
        };

        var validator = new Validator();
        var errors = validator.Validate(product);

        Console.WriteLine("=== Product Information ===");
        Console.WriteLine($"ID: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Description: {product.Description}");
        Console.WriteLine();

        Console.WriteLine("=== Validation Results ===");

        if (errors.Count == 0)
        {
            Console.WriteLine("All validations passed successfully.");
        }
        else
        {
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
        }

        Console.ReadKey();
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public int Length { get; }
    public MaxLengthAttribute(int length) => Length = length;
}

public class Validator
{
    public List<string> Validate(object obj)
    {
        var errors = new List<string>();
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);

            // Check for [Required] attribute
            if (Attribute.IsDefined(property, typeof(RequiredAttribute)))
            {
                if (value == null || (value is string str && string.IsNullOrEmpty(str)))
                {
                    errors.Add($"{property.Name} is required.");
                }
            }

            // Check for [MaxLength] attribute
            var maxLengthAttr = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null && value is string strValue && strValue.Length > maxLengthAttr.Length)
            {
                errors.Add($"{property.Name} exceeds the maximum length of {maxLengthAttr.Length}.");
            }
        }

        return errors;
    }
}

public class Product
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public required string Description { get; set; }
}


/*
* 1.Attribute-Based Validation:

    RequiredAttribute checks if the property is null or an empty string.

    MaxLengthAttribute checks if the string length exceeds the specified limit.

* 2.Validator Class with Reflection:

    Validate uses GetProperties() to retrieve properties.

    Attribute.IsDefined and GetCustomAttribute check for specific attributes, adding errors to the list if validations fail.
 
 */