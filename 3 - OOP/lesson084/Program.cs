public class Program
{
    static void Main()
    {
        Console.WriteLine("=== MOVEMENT SIMULATION ===");
        Console.WriteLine();

        IMovable car = new Car();
        IMovable person = new Person();

        car.Move(120);     // Car moves 120 meters
        person.Move(50);   // Person walks 50 meters

        Console.WriteLine("Movement Results:");
        Console.WriteLine("------------------");

        Console.WriteLine($"Car total distance: {((Car)car).TotalDistance} meters");
        Console.WriteLine($"Person total steps: {((Person)person).Steps} steps");

        Console.ReadKey();
    }
}

public interface IMovable
{
    void Move(double distance);
}

public class Car : IMovable
{
    public double TotalDistance { get; private set; }

    public void Move(double distance)
    {
        TotalDistance += distance;
    }
}

public class Person : IMovable
{
    public double Steps { get; private set; }
    private const double StepsPerMeter = 1.5;

    public void Move(double distance)
    {
        Steps += distance * StepsPerMeter;
    }
}

/*
* 1.Interface Definition:

    public interface IMovable defines the IMovable interface.

    void Move(double distance); declares the Move method signature.

* 2.Car Class Implementation:

    public class Car : IMovable indicates that Car implements IMovable.

    TotalDistance property keeps track of the distance the car has moved.

    The Move method adds the distance to TotalDistance.

* 3.Person Class Implementation:

    public class Person : IMovable indicates that Person implements IMovable.

    Steps property keeps track of the steps taken by the person.

    StepsPerMeter is a constant value set to 1.5.

    The Move method calculates steps by multiplying distance by StepsPerMeter.

* 4.Access Modifiers:

    TotalDistance and Steps are marked with private set to prevent external modification, ensuring encapsulation.
 */