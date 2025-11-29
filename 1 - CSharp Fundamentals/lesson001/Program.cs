public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Circle Area Calculator ===\n");

        Console.Write("Enter the radius of the circle: ");
        string? input = Console.ReadLine();

        if (double.TryParse(input, out double radius) && radius > 0)
        {
            Circle circle = new Circle();
            double area = circle.CalculateArea(radius);
            Console.WriteLine($"\nThe area of the circle with radius {radius} is {area:F2}");
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter a positive number.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}


public class Circle
{
    public double CalculateArea(double radius)
    {
        return Math.PI * radius * radius;
    }
}

/*
 !This exercise helps to understand how to use constants and perform simple mathematical calculations.

    * 1.Area Calculation:

    Using Math.PI for an accurate π value in calculations.

    * 2.Returning the Result:

    The method should return the calculated area in double format to accommodate decimal values.
 */