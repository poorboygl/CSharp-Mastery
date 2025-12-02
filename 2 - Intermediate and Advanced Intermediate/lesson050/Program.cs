public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Rectangle Analyzer ===");

        var rectangle1 = new Rectangle { Length = 10, Width = 10 };
        var rectangle2 = new Rectangle { Length = 15, Width = 8 };
        var rectangle3 = new Rectangle { Length = 6, Width = 12 };

        Console.Write("Rectangle 1: ");
        RectangleAnalyzer.AnalyzeRectangle(rectangle1);

        Console.Write("Rectangle 2: ");
        RectangleAnalyzer.AnalyzeRectangle(rectangle2);

        Console.Write("Rectangle 3: ");
        RectangleAnalyzer.AnalyzeRectangle(rectangle3);

        Console.ReadKey();
    }
}

public class Rectangle
{
    public double Length { get; set; }
    public double Width { get; set; }

    public (double length, double width) GetDimensions()
    {
        return (Length, Width);
    }

    public bool IsSquare()
    {
        return Length == Width;
    }
}

public static class RectangleAnalyzer
{
    public static void AnalyzeRectangle(Rectangle rectangle)
    {
        var (length, width) = rectangle.GetDimensions();

        string result = (length, width) switch
        {
            var _ when rectangle.IsSquare() => "Square",
            var _ when length > width => "Long Rectangle",
            var _ when width > length => "Wide Rectangle",
            _ => "Equal Rectangle"
        };

        Console.WriteLine(result);
    }
}

/*
 === Rectangle Analyzer ===
Rectangle 1: Square
Rectangle 2: Long Rectangle
Rectangle 3: Wide Rectangle
 */

/*
* 1.GetDimensions and IsSquare:

    GetDimensions returns a tuple with length and width.

    IsSquare checks if Length equals Width.

* 2.Pattern Matching in AnalyzeRectangle:

    AnalyzeRectangle uses switch with pattern matching to determine and print the type of rectangle.
 
 */