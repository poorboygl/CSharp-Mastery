using System;

// ======================================================================
// PROGRAM DEMO – SPECIFICATION PATTERN (Customer Filter)
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== SPECIFICATION PATTERN DEMO ===");

        var customer = new Customer
        {
            Name = "John Doe",
            Age = 25,
            IsVIP = true,
            Address = new CustomerAddress("123 Main St", "New York", "10001")
        };

        // Tạo các tiêu chí (specifications)
        var adultSpec = new IsAdultSpecification();
        var vipSpec = new IsVIPCustomerSpecification();
        var adultAndVipSpec = new AndSpecification(adultSpec, vipSpec);

        // Demo
        Console.WriteLine($"Is Adult? {adultSpec.IsSatisfiedBy(customer)}");
        Console.WriteLine($"Is VIP? {vipSpec.IsSatisfiedBy(customer)}");
        Console.WriteLine($"Is Adult AND VIP? {adultAndVipSpec.IsSatisfiedBy(customer)}");

        Console.WriteLine("===============================");
        Console.ReadKey();
    }
}

// ======================================================================
// VALUE OBJECT – CUSTOMER ADDRESS
// ======================================================================
public class CustomerAddress
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }

    public CustomerAddress(string street, string city, string zipCode)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    // Equality based on property values
    public override bool Equals(object obj)
    {
        if (obj is not CustomerAddress other) return false;
        return Street == other.Street && City == other.City && ZipCode == other.ZipCode;
    }

    public override int GetHashCode() => HashCode.Combine(Street, City, ZipCode);
}

// ======================================================================
// ENTITY – CUSTOMER
// ======================================================================
public class Customer
{
    public required string Name { get; set; }
    public int Age { get; set; }
    public required CustomerAddress Address { get; set; }
    public bool IsVIP { get; set; }
}

// ======================================================================
// SPECIFICATION INTERFACE
// ======================================================================
public interface ICustomerSpecification
{
    bool IsSatisfiedBy(Customer customer);
}

// ======================================================================
// SPECIFICATION IMPLEMENTATIONS
// ======================================================================
public class IsAdultSpecification : ICustomerSpecification
{
    public bool IsSatisfiedBy(Customer customer) => customer.Age >= 18;
}

public class IsVIPCustomerSpecification : ICustomerSpecification
{
    public bool IsSatisfiedBy(Customer customer) => customer.IsVIP;
}

// ======================================================================
// COMPOSITE SPECIFICATION (AND)
// ======================================================================
public class AndSpecification : ICustomerSpecification
{
    private readonly ICustomerSpecification _spec1;
    private readonly ICustomerSpecification _spec2;

    public AndSpecification(ICustomerSpecification spec1, ICustomerSpecification spec2)
    {
        _spec1 = spec1;
        _spec2 = spec2;
    }

    public bool IsSatisfiedBy(Customer customer)
        => _spec1.IsSatisfiedBy(customer) && _spec2.IsSatisfiedBy(customer);
}

/*
 !=== SPECIFICATION PATTERN DEMO ===
Is Adult? True
Is VIP? True
Is Adult AND VIP? True
===============================
 */

/*
* 1.CustomerAddress: A value object representing an address, implementing immutability and equality based on its properties.

* 2.Customer: The entity that contains properties such as Name, Age, and IsVIP.

* 3.Specifications:

    IsAdultSpecification checks if a customer is 18 or older.

    IsVIPCustomerSpecification checks if a customer is marked as VIP.

    AndSpecification combines two specifications using an AND logic, allowing complex rule composition.
 */