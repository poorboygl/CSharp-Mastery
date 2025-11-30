public class Program
{
    static void Main()
    {
        var processor = new FileProcessor();

        string path = "test.txt";   // Example file path

        Console.WriteLine("=== CountLinesInFile Result ===");
        Console.WriteLine("File Path: " + path);

        int lineCount = processor.CountLinesInFile(path);

        Console.WriteLine("Number of Lines: " + lineCount);

        Console.ReadKey();
    }
}

public class FileProcessor
{
    public int CountLinesInFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return 0;
        }

        string[] lines = File.ReadAllLines(filePath);
        return lines.Length;
    }
}

/*
* 1.File Existence Check:

File.Exists(filePath) returns false if the file does not exist, and in this case, we return 0.

* 2.Read All Lines and Count:

File.ReadAllLines(filePath) reads each line of the file into an array. lines.Length gives the count of lines.
 */