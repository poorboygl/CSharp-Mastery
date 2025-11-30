public class Program
{
    static void Main()
    {
        var documents = new Queue<string>();
        documents.Enqueue("Document1.pdf");
        documents.Enqueue("Document2.docx");
        documents.Enqueue("Document3.pptx");

        var simulator = new PrintQueueSimulator();

        Console.WriteLine("=== PrintQueueSimulator Result ===");
        Console.WriteLine("Documents in Queue: " + string.Join(", ", documents));

        int printedCount = simulator.PrintDocuments(documents);

        Console.WriteLine("Total Documents Printed: " + printedCount);

        Console.ReadKey();
    }
}

public class PrintQueueSimulator
{
    public int PrintDocuments(Queue<string> documents)
    {
        int count = 0;

        while (documents.Count > 0)
        {
            string document = documents.Dequeue();
            Console.WriteLine($"Printing {document}");
            count++;
        }

        return count;
    }
}

/*
* 1.Initialize Document Counter:

count starts at 0 to keep track of the number of printed documents.

* 2.Loop Through Queue:

As long as documents has items, Dequeue each document and print it.

* 3.Return Total Count:

After all documents are processed, count holds the total number of printed documents.
 */