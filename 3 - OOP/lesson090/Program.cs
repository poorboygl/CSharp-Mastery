public class Program
{
    static void Main()
    {
        Console.WriteLine("=== VECTOR OPERATOR DEMO ===\n");

        var v1 = new Vector(3, 4);
        var v2 = new Vector(1, 2);

        Console.WriteLine($"Vector 1: {v1}");
        Console.WriteLine($"Vector 2: {v2}\n");

        // Add
        var sum = v1 + v2;
        Console.WriteLine($"v1 + v2 = {sum}");

        // Subtract
        var diff = v1 - v2;
        Console.WriteLine($"v1 - v2 = {diff}");

        Console.ReadKey();
    }
}

public class Vector
{
    public double X { get; set; }
    public double Y { get; set; }

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }

    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

/*
* 1.Constructor:

    The constructor initializes X and Y for the vector.

* 2.Overloading the + Operator:

    public static Vector operator +(Vector a, Vector b) adds the X and Y components of two vectors and returns a new Vector.

* 3.Overloading the - Operator:

    public static Vector operator -(Vector a, Vector b) subtracts the X and Y components of two vectors and returns a new Vector.

* 4.Overriding ToString:

    public override string ToString() provides a custom string representation of the vector in the format "(X, Y)".
 */
