using System.Text;
public class Program
{
    static void Main()
    {
        var editor = new TextEditor();

        Console.WriteLine("=== INSERT TEXT ===");
        editor.Insert("Hello", 0);
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== INSERT MORE TEXT ===");
        editor.Insert(" World", 5);
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== DELETE TEXT ===");
        editor.Delete(5, 6);
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== UNDO DELETE ===");
        editor.Undo();
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== REPLACE TEXT ===");
        editor.Replace("C#", 6);
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== UNDO REPLACE ===");
        editor.Undo();
        Console.WriteLine(editor.GetContent());

        Console.WriteLine("=== DONE ===");
        Console.ReadKey();
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}

public class InsertTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly string _text;
    private readonly int _position;

    public InsertTextCommand(TextEditor editor, string text, int position)
    {
        _editor = editor;
        _text = text;
        _position = position;
    }

    public void Execute()
    {
        _editor.GetContentBuilder().Insert(_position, _text);
    }

    public void Undo()
    {
        _editor.GetContentBuilder().Remove(_position, _text.Length);
    }
}

public class DeleteTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly int _position;
    private readonly int _length;
    private string _deletedText;

    public DeleteTextCommand(TextEditor editor, int position, int length)
    {
        _editor = editor;
        _position = position;
        _length = length;
    }

    public void Execute()
    {
        _deletedText = _editor.GetContentBuilder().ToString(_position, _length);
        _editor.GetContentBuilder().Remove(_position, _length);
    }

    public void Undo()
    {
        _editor.GetContentBuilder().Insert(_position, _deletedText);
    }
}

public class ReplaceTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly string _newText;
    private readonly int _position;
    private string _oldText;

    public ReplaceTextCommand(TextEditor editor, string newText, int position)
    {
        _editor = editor;
        _newText = newText;
        _position = position;
    }

    public void Execute()
    {
        _oldText = _editor.GetContentBuilder().ToString(_position, _newText.Length);
        _editor.GetContentBuilder().Remove(_position, _newText.Length);
        _editor.GetContentBuilder().Insert(_position, _newText);
    }

    public void Undo()
    {
        _editor.GetContentBuilder().Remove(_position, _newText.Length);
        _editor.GetContentBuilder().Insert(_position, _oldText);
    }
}

public class TextEditor
{
    private readonly StringBuilder _content = new();
    private readonly Stack<ICommand> _commands = new();

    public StringBuilder GetContentBuilder() => _content;
    public string GetContent() => _content.ToString();

    public void Insert(string text, int position)
    {
        var command = new InsertTextCommand(this, text, position);
        command.Execute();
        _commands.Push(command);
    }

    public void Delete(int position, int length)
    {
        var command = new DeleteTextCommand(this, position, length);
        command.Execute();
        _commands.Push(command);
    }

    public void Replace(string newText, int position)
    {
        var command = new ReplaceTextCommand(this, newText, position);
        command.Execute();
        _commands.Push(command);
    }

    public void Undo()
    {
        if (_commands.Count > 0)
        {
            var lastCommand = _commands.Pop();
            lastCommand.Undo();
        }
    }
}

/*
* 1.Command Classes with Execute and Undo:

    InsertTextCommand, DeleteTextCommand, and ReplaceTextCommand each implement the ICommand interface.

    Each command’s Execute and Undo methods manipulate TextEditor’s content accordingly.

* 2.TextEditor with Command Management:

    TextEditor keeps a Stack<ICommand> to store commands for undo functionality.

    Each editing operation (Insert, Delete, Replace) is encapsulated as a command and stored in _commands.
 */