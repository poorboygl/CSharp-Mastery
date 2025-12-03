using System.Reflection;
public class Program
{
    static void Main()
    {
        Console.WriteLine("===== SERVICE FACTORY DEMO =====");

        var factory = new ServiceFactory();

        string[] servicesToRun = { "Email", "SMS", "Push" };

        foreach (var serviceName in servicesToRun)
        {
            Console.WriteLine($"\n--- Executing Service: {serviceName} ---");
            try
            {
                var service = factory.GetService(serviceName);
                service.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.WriteLine("\n===== END OF PROGRAM =====");
        Console.ReadKey();
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute : Attribute
{
    public string ServiceName { get; }

    public ServiceAttribute(string serviceName)
    {
        ServiceName = serviceName;
    }
}

public interface IService
{
    void Execute();
}

[Service("Email")]
public class EmailService : IService
{
    public void Execute()
    {
        Console.WriteLine("Executing Email Service");
    }
}

[Service("SMS")]
public class SmsService : IService
{
    public void Execute()
    {
        Console.WriteLine("Executing SMS Service");
    }
}

[Service("Push")]
public class PushNotificationService : IService
{
    public void Execute()
    {
        Console.WriteLine("Executing Push Notification Service");
    }
}

public class ServiceFactory
{
    public IService GetService(string serviceName)
    {
        var serviceType = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .FirstOrDefault(t =>
                                      t.GetCustomAttributes<ServiceAttribute>()
                                       .Any(attr => attr.ServiceName == serviceName) &&
                                      typeof(IService).IsAssignableFrom(t));

        if (serviceType == null)
        {
            throw new InvalidOperationException($"No service found with name '{serviceName}'");
        }

        return (IService)Activator.CreateInstance(serviceType);
    }
}

/*
* 1.Custom ServiceAttribute:

ServiceAttribute is used to tag classes with a ServiceName.

* 2.Service Implementations:

EmailService, SmsService, and PushNotificationService are decorated with ServiceAttribute and implement IService.

* 3.ServiceFactory with Reflection:

GetService uses reflection to find the type that matches the serviceName and creates an instance using Activator.CreateInstance.

Throws an exception if no matching service is found.
 
 */