// ======================================================================
// PROGRAM ENTRY – EVENT SOURCING PRODUCT DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== PRODUCT EVENT SOURCING DEMO ===\n");

        var eventStore = new EventStore();
        var handler = new ProductCommandHandler(eventStore);
        var projection = new ProductProjection();

        // Create Product
        var id = Guid.NewGuid();
        handler.Handle(new CreateProductCommand(id, "Laptop ABC", 1500));

        // Update Product
        handler.Handle(new UpdateProductCommand(id, "Laptop ABC - New Version", 1700));

        // Replay events into projection
        projection.Replay(eventStore.GetEvents());

        // Query read model
        var product = projection.GetProduct(id);

        Console.WriteLine("\n=== CURRENT PRODUCT STATE (READ MODEL) ===");
        Console.WriteLine($"ID: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Price: {product.Price}");

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// MARKER INTERFACES
// ======================================================================
public interface ICommand { }
public interface IEvent { }


// ======================================================================
// COMMANDS
// ======================================================================
public class CreateProductCommand : ICommand
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public CreateProductCommand(Guid productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
    }
}

public class UpdateProductCommand : ICommand
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public UpdateProductCommand(Guid productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
    }
}


// ======================================================================
// EVENTS
// ======================================================================
public class ProductCreatedEvent : IEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductCreatedEvent(Guid productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
    }
}

public class ProductUpdatedEvent : IEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductUpdatedEvent(Guid productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
    }
}


// ======================================================================
// EVENT STORE
// ======================================================================
public class EventStore
{
    private readonly List<IEvent> _events = new();

    public void SaveEvent(IEvent @event) => _events.Add(@event);

    public IEnumerable<IEvent> GetEvents() => _events;
}


// ======================================================================
// COMMAND HANDLER
// ======================================================================
public class ProductCommandHandler
{
    private readonly EventStore _eventStore;

    public ProductCommandHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public void Handle(CreateProductCommand command)
    {
        var ev = new ProductCreatedEvent(command.ProductId, command.Name, command.Price);
        _eventStore.SaveEvent(ev);
    }

    public void Handle(UpdateProductCommand command)
    {
        var ev = new ProductUpdatedEvent(command.ProductId, command.Name, command.Price);
        _eventStore.SaveEvent(ev);
    }
}


// ======================================================================
// READ MODEL PROJECTION
// ======================================================================
public class ProductProjection
{
    private readonly Dictionary<Guid, ProductState> _productStates = new();

    public void Apply(IEvent @event)
    {
        switch (@event)
        {
            case ProductCreatedEvent created:
                _productStates[created.ProductId] = new ProductState
                {
                    Id = created.ProductId,
                    Name = created.Name,
                    Price = created.Price
                };
                break;

            case ProductUpdatedEvent updated:
                if (_productStates.TryGetValue(updated.ProductId, out var product))
                {
                    product.Name = updated.Name;
                    product.Price = updated.Price;
                }
                break;
        }
    }

    public void Replay(IEnumerable<IEvent> events)
    {
        foreach (var e in events)
            Apply(e);
    }

    public ProductState GetProduct(Guid productId)
    {
        _productStates.TryGetValue(productId, out var product);
        return product;
    }
}


// ======================================================================
// READ MODEL STATE
// ======================================================================
public class ProductState
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

/*
 === PRODUCT EVENT SOURCING DEMO ===


=== CURRENT PRODUCT STATE (READ MODEL) ===
ID: 95a03683-42e8-4fe4-bcd4-1d1938eca291
Name: Laptop ABC - New Version
Price: 1700

=== DONE ===
 */

/*
* 1.Command and Event Interfaces: ICommand and IEvent marker interfaces distinguish commands from events.

* 2.Commands and Events: CreateProductCommand and UpdateProductCommand represent requests, while ProductCreatedEvent and ProductUpdatedEvent capture state changes.

* 3.EventStore: Stores all events, allowing them to be replayed to rebuild the state.

* 4.ProductProjection: The read model that applies events to create a current view of each product.

* 5.ProductCommandHandler: Handles commands and produces the respective events, storing them in the EventStore.
 */