namespace Coding.Exercise
{
    public class Program
    {
        static void Main()
        {
            var factory = new ServiceFactory();
            var manager = new ServiceManager(factory);

            manager.RunService("EmailService");
            manager.RunService("SmsService");

            Console.ReadKey();
        }
    }

    public interface IService
    {
        void Execute();
    }

    public class EmailService : IService
    {
        public void Execute()
        {
            Console.WriteLine("Email service executed");
        }
    }

    public class SmsService : IService
    {
        public void Execute()
        {
            Console.WriteLine("SMS service executed");
        }
    }

    public class ServiceFactory
    {
        public IService GetService(string serviceName)
        {
            var assembly = typeof(ServiceFactory).Assembly;
            var type = assembly.GetType($"Coding.Exercise.{serviceName}");

            if (type == null || !typeof(IService).IsAssignableFrom(type))
                throw new ArgumentException("Invalid service name provided.");

            return (IService)Activator.CreateInstance(type);
        }
    }

    public class ServiceManager
    {
        private readonly ServiceFactory _serviceFactory;

        public ServiceManager(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public void RunService(string serviceName)
        {
            var service = _serviceFactory.GetService(serviceName);
            service.Execute();
        }
    }
}

/*
 * 1.Dynamic Service Creation Using Reflection:

        ServiceFactory.GetService uses Type.GetType() and Activator.CreateInstance() to dynamically create an instance of the requested service.

        Checks are in place to throw an exception if serviceName does not correspond to a valid IService implementation.

* 2.Dependency Injection:

    ServiceManager receives ServiceFactory as a dependency, promoting loose coupling.

    RunService retrieves the correct service from the factory and calls Execute, making ServiceManager independent of specific service types.
 */