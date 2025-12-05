using System;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== VEHICLE FACTORY DEMO ===\n");

        // Show Car specifications
        Console.WriteLine("Car Factory:");
        var carShowroom = new VehicleShowroom(new CarFactory());
        carShowroom.DisplaySpecifications();

        Console.WriteLine();

        // Show Truck specifications
        Console.WriteLine("Truck Factory:");
        var truckShowroom = new VehicleShowroom(new TruckFactory());
        truckShowroom.DisplaySpecifications();

        Console.ReadKey();
    }
}


public abstract class Vehicle
{
    public abstract string GetSpecifications();
}

public class Car : Vehicle
{
    public override string GetSpecifications()
    {
        return "Specifications: Small size, 4 seats, max speed 200 km/h.";
    }
}

public class Truck : Vehicle
{
    public override string GetSpecifications()
    {
        return "Specifications: Large size, 2 seats, max speed 120 km/h.";
    }
}

public abstract class VehicleFactory
{
    public abstract Vehicle CreateVehicle();
}

public class CarFactory : VehicleFactory
{
    public override Vehicle CreateVehicle()
    {
        return new Car();
    }
}

public class TruckFactory : VehicleFactory
{
    public override Vehicle CreateVehicle()
    {
        return new Truck();
    }
}

public class VehicleShowroom
{
    private readonly VehicleFactory _factory;

    public VehicleShowroom(VehicleFactory factory)
    {
        _factory = factory;
    }

    public void DisplaySpecifications()
    {
        Vehicle vehicle = _factory.CreateVehicle();
        Console.WriteLine(vehicle.GetSpecifications());
    }
}

/*
 === VEHICLE FACTORY DEMO ===

Car Factory:
Specifications: Small size, 4 seats, max speed 200 km/h.

Truck Factory:
Specifications: Large size, 2 seats, max speed 120 km/h.
*/

/*
* 1.Abstract Classes and Methods:

    Vehicle and VehicleFactory are abstract, defining the structure but leaving implementation to subclasses.

* 2.Concrete Vehicle and Factory Classes:

    Car and Truck implement GetSpecifications to return their specifications.

    CarFactory and TruckFactory implement CreateVehicle to create Car and Truck objects, respectively.

* 3.VehicleShowroom Class:

    VehicleShowroom is initialized with a VehicleFactory.

    DisplaySpecifications uses the factory to create a vehicle and print its specifications, demonstrating polymorphism and abstraction.
 
 */