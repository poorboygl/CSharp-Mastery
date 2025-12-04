public class Program
{
    static void Main()
    {
        Console.WriteLine("=== SHAPE AREA CALCULATOR ===");
        Console.WriteLine();

        Shape rectangle = new Rectangle { Width = 5, Height = 10 };
        Shape circle = new Circle { Radius = 4 };
        Shape triangle = new Triangle { Base = 6, Height = 8 };

        Console.WriteLine("Rectangle:");
        ShapeDrawer.DrawShapeArea(rectangle);
        Console.WriteLine();

        Console.WriteLine("Circle:");
        ShapeDrawer.DrawShapeArea(circle);
        Console.WriteLine();

        Console.WriteLine("Triangle:");
        ShapeDrawer.DrawShapeArea(triangle);

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

public class Triangle : Shape
{
    public double Base { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return 0.5 * Base * Height;
    }
}

public class ShapeDrawer
{
    public static void DrawShapeArea(Shape shape)
    {
        Console.WriteLine($"The area is {shape.CalculateArea()}");
    }
}

/*
* 1.Triangle Class Implementation:

    Triangle inherits from Shape.

    It overrides CalculateArea with return 0.5 * Base * Height;.

* 2.ShapeDrawer Class:

    DrawShapeArea is a static method that accepts any Shape and calls its CalculateArea method.

    Console.WriteLine is used to print the area.

* 3.Polymorphism in Action:

    DrawShapeArea accepts any Shape object, enabling polymorphism. The method calls CalculateArea on each shape instance, demonstrating that derived classes can provide different behavior.
 */
