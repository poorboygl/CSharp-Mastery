public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Animal Shelter Demo ===\n");

        AnimalShelter shelter = new AnimalShelter();

        shelter.AddAnimal(new Dog("Buddy", 3));
        shelter.AddAnimal(new Cat("Milo", 2));
        shelter.AddAnimal(new Cow("Daisy", 5));

        shelter.ListAllSounds();

        Console.ReadKey();
    }
}
public abstract class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }

    protected Animal(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public abstract string MakeSound();
}

public class Dog : Animal
{
    public Dog(string name, int age) : base(name, age) { }

    public override string MakeSound()
    {
        return "Woof!";
    }
}

public class Cat : Animal
{
    public Cat(string name, int age) : base(name, age) { }

    public override string MakeSound()
    {
        return "Meow!";
    }
}

public class Cow : Animal
{
    public Cow(string name, int age) : base(name, age) { }

    public override string MakeSound()
    {
        return "Moo!";
    }
}

public class AnimalShelter
{
    private readonly List<Animal> _animals = new List<Animal>();

    public void AddAnimal(Animal animal)
    {
        _animals.Add(animal);
    }

    public void ListAllSounds()
    {
        foreach (var animal in _animals)
        {
            Console.WriteLine($"{animal.Name} says: {animal.MakeSound()}");
        }
    }
}

/*
* 1.Animal Class:

    Animal is an abstract base class with properties Name and Age.

    MakeSound is an abstract method implemented by each derived class.

* 2.Dog, Cat, and Cow Classes:

    Each animal class inherits from Animal and provides a specific sound through MakeSound.

* 3.AnimalShelter Class:

    Holds a list of Animal objects, allowing for any type of animal.

    ListAllSounds calls MakeSound polymorphically, displaying the appropriate sound for each animal.
 */