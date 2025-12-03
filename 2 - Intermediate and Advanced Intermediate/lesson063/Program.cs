public class Program
{
    static void Main()
    {
        Console.WriteLine("===== LOGGER SINGLETON DEMO =====\n");

        // Lấy logger từ factory
        var factory = new ServiceFactory();
        ILogger logger = factory.GetLogger();

        // Tạo service và sử dụng logger
        var reportService = new ReportService(logger);

        Console.WriteLine("--- Generating Reports ---");
        reportService.GenerateReport("Sales Q1 2025");
        reportService.GenerateReport("Inventory Report 2025");
        reportService.GenerateReport("Employee Performance");

        Console.WriteLine("\n===== END OF PROGRAM =====");
        Console.ReadKey();
    }
}

public interface ILogger
{
    void Log(string message);
}

public class Logger : ILogger
{
    // Using Lazy<T> for thread-safe, lazy initialization
    private static readonly Lazy<Logger> _instance = new(() => new Logger());

    // Private constructor to prevent direct instantiation
    private Logger() { }

    // Public accessor for the singleton instance
    public static Logger Instance => _instance.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}

public class ServiceFactory
{
    // Returns the singleton instance of Logger
    public ILogger GetLogger()
    {
        return Logger.Instance;
    }
}

public class ReportService
{
    private readonly ILogger _logger;

    public ReportService(ILogger logger)
    {
        _logger = logger;
    }

    public void GenerateReport(string reportData)
    {
        _logger.Log($"Generating report: {reportData}");
    }
}