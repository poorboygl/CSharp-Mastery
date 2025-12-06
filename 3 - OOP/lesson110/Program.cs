public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Support Request Handling (Chain of Responsibility) ===\n");

        // Tạo chain:
        // Basic -> Technical -> Billing -> General
        ISupportHandler handlerChain =
            new BasicSupportHandler(
                new TechnicalSupportHandler(
                    new BillingSupportHandler(
                        new GeneralSupportHandler()
                    )
                )
            );

        Console.WriteLine("[Test 1] Basic issue:");
        handlerChain.HandleRequest("basic");

        Console.WriteLine("\n[Test 2] Technical issue:");
        handlerChain.HandleRequest("technical");

        Console.WriteLine("\n[Test 3] Billing issue:");
        handlerChain.HandleRequest("billing");

        Console.WriteLine("\n[Test 4] Unknown issue:");
        handlerChain.HandleRequest("something-else");

        Console.ReadKey();
    }
}

public interface ISupportHandler
{
    void HandleRequest(string issueType);
}

public abstract class SupportHandler : ISupportHandler
{
    protected SupportHandler nextHandler;

    protected SupportHandler(SupportHandler nextHandler)
    {
        this.nextHandler = nextHandler;
    }

    public virtual void HandleRequest(string issueType)
    {
        if (nextHandler != null)
        {
            nextHandler.HandleRequest(issueType);
        }
    }
}

public class BasicSupportHandler : SupportHandler
{
    public BasicSupportHandler(SupportHandler nextHandler) : base(nextHandler) { }

    public override void HandleRequest(string issueType)
    {
        if (issueType == "basic")
        {
            Console.WriteLine("Basic support handling: Resolving basic issue");
        }
        else
        {
            base.HandleRequest(issueType);
        }
    }
}

public class TechnicalSupportHandler : SupportHandler
{
    public TechnicalSupportHandler(SupportHandler nextHandler) : base(nextHandler) { }

    public override void HandleRequest(string issueType)
    {
        if (issueType == "technical")
        {
            Console.WriteLine("Technical support handling: Resolving technical issue");
        }
        else
        {
            base.HandleRequest(issueType);
        }
    }
}

public class BillingSupportHandler : SupportHandler
{
    public BillingSupportHandler(SupportHandler nextHandler) : base(nextHandler) { }

    public override void HandleRequest(string issueType)
    {
        if (issueType == "billing")
        {
            Console.WriteLine("Billing support handling: Resolving billing issue");
        }
        else
        {
            base.HandleRequest(issueType);
        }
    }
}

public class GeneralSupportHandler : SupportHandler
{
    public GeneralSupportHandler() : base(null) { }

    public override void HandleRequest(string issueType)
    {
        Console.WriteLine("General support handling: Forwarding to a support representative");
    }
}

/*
* 1.ISupportHandler Interface:

    Defines HandleRequest for processing customer support issues.

* 2.SupportHandler Abstract Class:

    Holds a reference to the next handler and forwards requests if they aren’t handled.

* 3.Concrete Handlers:

    Each handler checks the issue type and either handles it or passes it to the next handler.
 */