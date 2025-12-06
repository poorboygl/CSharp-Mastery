public class Program
{
    static void Main()
    {
        Console.WriteLine("===== VEHICLE MOVEMENT STRATEGY DEMO =====\n");

        // Tạo phương tiện với hành vi ban đầu (Drive)
        Vehicle car = new Vehicle(new DriveBehavior());
        Console.Write("Car: ");
        car.PerformMove();

        // Thay đổi hành vi sang Fly
        Console.Write("\nCar changes behavior -> ");
        car.SetMovementBehavior(new FlyBehavior());
        car.PerformMove();

        // Tạo phương tiện khác với Sail behavior
        Vehicle boat = new Vehicle(new SailBehavior());
        Console.Write("\nBoat: ");
        boat.PerformMove();

        Console.WriteLine("\n===== END =====");

        Console.ReadKey();
    }
}

public interface IMovementBehavior
{
    void Move();
}

public class DriveBehavior : IMovementBehavior
{
    public void Move()
    {
        Console.WriteLine("Driving on the road");
    }
}

public class FlyBehavior : IMovementBehavior
{
    public void Move()
    {
        Console.WriteLine("Flying in the sky");
    }
}

public class SailBehavior : IMovementBehavior
{
    public void Move()
    {
        Console.WriteLine("Sailing on the water");
    }
}

public class Vehicle
{
    public IMovementBehavior MovementBehavior { get; private set; }

    public Vehicle(IMovementBehavior movementBehavior)
    {
        MovementBehavior = movementBehavior;
    }

    public void PerformMove()
    {
        MovementBehavior.Move();
    }

    public void SetMovementBehavior(IMovementBehavior newBehavior)
    {
        MovementBehavior = newBehavior;
    }
}

/*
 * 1.IMovementBehavior Interface:

    Defines the Move method, allowing each movement behavior to provide its unique implementation.

* 2.Concrete Movement Behaviors:

    DriveBehavior, FlyBehavior, and SailBehavior each implement Move, printing a movement-specific message.

* 3.Vehicle Class with Composition:

    Vehicle has an IMovementBehavior property, set via the constructor and changeable with SetMovementBehavior.

    PerformMove calls Move on the current MovementBehavior, following the strategy pattern.
 */
