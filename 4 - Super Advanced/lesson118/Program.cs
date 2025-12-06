public class Program
{
    static void Main()
    {
        Console.Title = "Chat Room Mediator Demo";

        Console.WriteLine("=== CHAT ROOM MEDIATOR PATTERN DEMO ===\n");

        var chatRoom = new ChatRoom();

        var user1 = new User("Alice");
        var user2 = new User("Bob");
        var user3 = new User("Charlie");

        chatRoom.RegisterUser(user1);
        chatRoom.RegisterUser(user2);
        chatRoom.RegisterUser(user3);

        user1.Send("Hello everyone!");
        user2.Send("Hi Alice!");
        user3.Send("Good evening folks!");

        Console.ReadKey();
    }
}

public interface IChatMediator
{
    void SendMessage(string message, User sender);
}

public class ChatRoom : IChatMediator
{
    private readonly List<User> _users = new();

    public void RegisterUser(User user)
    {
        _users.Add(user);
        user.ChatRoom = this;
    }

    public void SendMessage(string message, User sender)
    {
        foreach (var user in _users)
        {
            if (user != sender)
            {
                user.Receive(message, sender);
            }
        }
    }
}

public class User
{
    public string Name { get; }
    public IChatMediator ChatRoom { get; set; }

    public User(string name)
    {
        Name = name;
    }

    public void Send(string message)
    {
        ChatRoom?.SendMessage(message, this);
    }

    public void Receive(string message, User sender)
    {
        Console.WriteLine($"{Name} received a message from {sender.Name}: {message}");
    }
}

/*
* 1.ChatRoom as Mediator:

    ChatRoom implements IChatMediator and keeps a list of registered users.

    It uses SendMessage to relay messages to all users except the sender.

* 2.User Sending and Receiving Messages:

    User.Send sends messages via the mediator.

    User.Receive displays the received message, identifying the sender
 
 */
