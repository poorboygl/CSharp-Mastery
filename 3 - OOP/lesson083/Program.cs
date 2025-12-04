public class Program
{
    static void Main()
    {
        Console.WriteLine("=== AREA CALCULATOR ===");
        Console.WriteLine();

        // Tạo danh sách các hình
        List<Shape> shapes = new List<Shape>
        {
            new Rectangle { Width = 5, Height = 10 },
            new Circle { Radius = 3 }
        };

        Console.WriteLine("Calculated Areas:");
        Console.WriteLine("-----------------");

        // Tính và in diện tích từng hình
        foreach (var shape in shapes)
        {
            Console.WriteLine($"Area = {shape.CalculateArea()}");
        }

        Console.ReadKey();
    }
}


public abstract class Shape
{
    public abstract double CalculateArea();
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return Width * Height;
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }
}

/*
* 1.Abstract Class and Method:

    public abstract class Shape defines an abstract class.

    public abstract double CalculateArea(); declares an abstract method.

* 2.Rectangle Class:

    Inherits from Shape using public class Rectangle : Shape.

    Defines Width and Height properties.

    Implements CalculateArea method with public override double CalculateArea().

* 3.Circle Class:

    Inherits from Shape.

    Defines Radius property.

    Implements CalculateArea method using the formula for the area of a circle.
 */