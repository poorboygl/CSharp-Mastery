public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Rectangle Perimeter Calculator ===\n");

        Console.Write("Enter the length of the rectangle: ");
        string? lengthInput = Console.ReadLine();

        Console.Write("Enter the width of the rectangle: ");
        string? widthInput = Console.ReadLine();

        if (double.TryParse(lengthInput, out double length) && length > 0 &&
            double.TryParse(widthInput, out double width) && width > 0)
        {
            Rectangle rectangle = new Rectangle();
            double perimeter = rectangle.CalculatePerimeter(length, width);
            Console.WriteLine($"\nThe perimeter of the rectangle with length {length} and width {width} is {perimeter:F2}");
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter positive numbers.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

public class Rectangle
{
    public double CalculatePerimeter(double length, double width)
    {
        return 2 * (length + width);
    }
}

/*
 !Explanation of the Solution
    * 1.Formula Application:

    The method calculates and returns the perimeter using 2 * (length + width) directly, ensuring the correct formula is applied.

    * 2.Return Type as double:

    Using double as the return type allows for decimal values, making the method flexible for various input sizes.
 */