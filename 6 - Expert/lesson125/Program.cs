public class Program
{
    static void Main()
    {
        Console.Title = "Proxy Pattern Demo";
        Console.WriteLine("=== PROXY PATTERN - DOCUMENT ACCESS CONTROL ===\n");

        IDocument secureDoc = new DocumentProxy("wrong_password", "Secret Contract Document");
        Console.WriteLine("Trying with wrong password:");
        secureDoc.DisplayContent();

        Console.WriteLine("\nTrying with correct password:");
        secureDoc = new DocumentProxy("correct_password", "Secret Contract Document");
        secureDoc.DisplayContent();

        Console.ReadKey();
    }
}

public interface IDocument
{
    void DisplayContent();
}

public class Document : IDocument
{
    private readonly string _content;

    public Document(string content)
    {
        _content = content;
    }

    public void DisplayContent()
    {
        Console.WriteLine($"Document Content: {_content}");
    }
}

public class DocumentProxy : IDocument
{
    private readonly string _password;
    private readonly string _content;
    private Document _document;

    public DocumentProxy(string password, string content)
    {
        _password = password;
        _content = content;
    }

    public void DisplayContent()
    {
        if (_document == null && CheckAccess())
        {
            _document = new Document(_content);
        }

        if (_document != null)
        {
            _document.DisplayContent();
        }
    }

    private bool CheckAccess()
    {
        if (_password == "correct_password")
        {
            return true;
        }

        Console.WriteLine("Access Denied");
        return false;
    }
}

/*
 !=== PROXY PATTERN - DOCUMENT ACCESS CONTROL ===

    Trying with wrong password:
    Access Denied

    Trying with correct password:
    Document Content: Secret Contract Document
 */

/*
* 1.Document Class:

    Document implements IDocument and simply holds and displays content.

* 2.DocumentProxy with Lazy Initialization and Access Control:

    DocumentProxy checks the password in CheckAccess.

    DisplayContent initializes the Document only when the password matches and content is accessed.
 */