public class Program
{
    static void Main()
    {
        Console.WriteLine("===== DOCUMENT GENERATOR SYSTEM =====\n");

        DocumentGenerator report = new ReportGenerator();
        DocumentGenerator invoice = new InvoiceGenerator();

        Console.WriteLine(">>> Generating Report...\n");
        report.GenerateDocument();

        Console.WriteLine("\n>>> Generating Invoice...\n");
        invoice.GenerateDocument();

        Console.WriteLine("\n===== END =====");

        Console.ReadKey();
    }
}
public abstract class DocumentGenerator
{
    public void GenerateDocument()
    {
        GenerateHeader();
        GenerateBody();
        GenerateFooter();
    }

    protected abstract void GenerateHeader();
    protected abstract void GenerateBody();
    protected abstract void GenerateFooter();
}

public class ReportGenerator : DocumentGenerator
{
    protected override void GenerateHeader()
    {
        Console.WriteLine("--- Report Header ---");
    }

    protected override void GenerateBody()
    {
        Console.WriteLine("This is the body of the report.");
    }

    protected override void GenerateFooter()
    {
        Console.WriteLine("--- Report Footer ---");
    }
}

public class InvoiceGenerator : DocumentGenerator
{
    protected override void GenerateHeader()
    {
        Console.WriteLine("=== Invoice Header ===");
    }

    protected override void GenerateBody()
    {
        Console.WriteLine("This is the body of the invoice.");
    }

    protected override void GenerateFooter()
    {
        Console.WriteLine("=== Invoice Footer ===");
    }
}

/*
* 1.DocumentGenerator Class:

    Defines the GenerateDocument template method that executes the workflow in a specific sequence: header, body, footer.

    Uses protected abstract methods for GenerateHeader, GenerateBody, and GenerateFooter, enforcing that subclasses provide their own implementations.

* 2.ReportGenerator and InvoiceGenerator Classes:

    Each class provides a unique implementation for GenerateHeader, GenerateBody, and GenerateFooter.

    Calling GenerateDocument on each subclass follows the same workflow but with different content.
 
 */