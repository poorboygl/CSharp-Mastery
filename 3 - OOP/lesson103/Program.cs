public class Program
{
    static void Main()
    {
        Console.WriteLine("===== GAME INVENTORY SYSTEM =====\n");

        var weaponInventory = new Inventory<Weapon>();
        var potionInventory = new Inventory<Potion>();

        Console.WriteLine(">>> Adding items...\n");

        weaponInventory.AddItem(new Weapon("Sword", 2, 50));
        weaponInventory.AddItem(new Weapon("Axe", 1, 70));
        weaponInventory.AddItem(new Weapon("Sword", 1, 50)); // restock sword

        potionInventory.AddItem(new Potion("Health Potion", 5, "Restore HP"));
        potionInventory.AddItem(new Potion("Mana Potion", 3, "Restore MP"));
        potionInventory.AddItem(new Potion("Health Potion", 2, "Restore HP")); // restock

        Console.WriteLine(">>> Current Weapons:");
        foreach (var w in weaponInventory.GetAllItems())
        {
            Console.WriteLine(w);
        }

        Console.WriteLine("\n>>> Current Potions:");
        foreach (var p in potionInventory.GetAllItems())
        {
            Console.WriteLine(p);
        }

        Console.WriteLine("\n>>> Removing some items...");
        weaponInventory.RemoveItem("Sword", 2);
        potionInventory.RemoveItem("Mana Potion", 3); // remove completely

        Console.WriteLine("\n>>> Final Weapons:");
        foreach (var w in weaponInventory.GetAllItems())
        {
            Console.WriteLine(w);
        }

        Console.WriteLine("\n>>> Final Potions:");
        foreach (var p in potionInventory.GetAllItems())
        {
            Console.WriteLine(p);
        }

        Console.WriteLine("\n===== END OF INVENTORY DEMO =====");
        Console.ReadKey();
    }
}

public class InventoryItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    public InventoryItem(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }

    public void Restock(int amount)
    {
        Quantity += amount;
    }
}

public class Weapon : InventoryItem
{
    public int AttackPower { get; set; }

    public Weapon(string name, int quantity, int attackPower)
        : base(name, quantity)
    {
        AttackPower = attackPower;
    }

    public override string ToString()
    {
        return $"{Name} - Quantity: {Quantity}, Attack Power: {AttackPower}";
    }
}

public class Potion : InventoryItem
{
    public string Effect { get; set; }

    public Potion(string name, int quantity, string effect)
        : base(name, quantity)
    {
        Effect = effect;
    }

    public override string ToString()
    {
        return $"{Name} - Quantity: {Quantity}, Effect: {Effect}";
    }
}

public class Inventory<T> where T : InventoryItem
{
    private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Name))
        {
            _items[item.Name].Restock(item.Quantity);
        }
        else
        {
            _items[item.Name] = item;
        }
    }

    public bool RemoveItem(string itemName, int quantity)
    {
        if (_items.TryGetValue(itemName, out var item))
        {
            if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
                return true;
            }
            else if (item.Quantity == quantity)
            {
                _items.Remove(itemName);
                return true;
            }
        }
        return false;
    }

    public T FindItemByName(string itemName)
    {
        _items.TryGetValue(itemName, out var item);
        return item;
    }

    public List<T> GetAllItems()
    {
        return _items.Values.ToList();
    }
}

/*
* 1.InventoryItem Class:

    InventoryItem has Name and Quantity properties and a Restock method to adjust quantity.

* 2.Weapon and Potion Classes:

    Weapon includes an AttackPower property, while Potion includes an Effect property.

    Both classes override ToString to improve readability during testing.

* 3.Inventory Class with Generics:

    AddItem increases the quantity of an existing item or adds a new item.

    RemoveItem decreases the item quantity and removes it if it reaches zero.
    
    FindItemByName retrieves an item by its name.

    GetAllItems returns a list of all items in the inventory.
 
 */