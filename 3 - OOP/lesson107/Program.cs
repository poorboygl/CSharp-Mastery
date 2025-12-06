public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Command Pattern Demo ===");

        var remote = new RemoteControl();

        // Ví dụ bật đèn
        remote.SetCommand(new LightOnCommand());
        Console.WriteLine("[Action] Pressing button for Light:");
        remote.PressButton();
        remote.PressUndo();

        Console.WriteLine();

        // Ví dụ bật quạt
        remote.SetCommand(new FanOnCommand());
        Console.WriteLine("[Action] Pressing button for Fan:");
        remote.PressButton();
        remote.PressUndo();

        Console.ReadKey();
    }
}


public interface ICommand
{
    void Execute();
    void Undo();
}

public class LightOnCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Light is on");
    }

    public void Undo()
    {
        Console.WriteLine("Light is off");
    }
}

public class FanOnCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Fan is on");
    }

    public void Undo()
    {
        Console.WriteLine("Fan is off");
    }
}

public class RemoteControl
{
    private ICommand? _command; // Make _command nullable to avoid warning

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void PressButton()
    {
        _command?.Execute();
    }

    public void PressUndo()
    {
        _command?.Undo();
    }
}

/*
* 1.ICommand Interface:

    Defines Execute and Undo methods, encapsulating each command with execution and undo functionality.

* 2.LightOnCommand and FanOnCommand:

    Implement Execute to turn on the device and Undo to turn it off, encapsulating each device’s specific actions.

* 3.RemoteControl Class with Command:

    SetCommand assigns a command to the remote.

    PressButton executes the command, and PressUndo undoes it, following the command pattern to decouple actions from the remote itself.
 
 */