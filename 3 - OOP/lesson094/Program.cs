public class Program
{
    static void Main()
    {
        var config = ConfigurationManager.Instance;

        config.SetSetting("AppName", "My Application");
        config.SetSetting("Version", "1.0.0");

        Console.WriteLine("AppName: " + config.GetSetting("AppName"));
        Console.WriteLine("Version: " + config.GetSetting("Version"));

        // Kiểm tra Singleton (hai biến phải cùng 1 instance)
        var config2 = ConfigurationManager.Instance;
        Console.WriteLine("Same instance? " + (config == config2));

        Console.ReadKey();
    }
}


public class ConfigurationManager
{
    private static readonly ConfigurationManager _instance = new ConfigurationManager();
    private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

    private ConfigurationManager() { }

    public static ConfigurationManager Instance => _instance;

    public void SetSetting(string key, string value)
    {
        _settings[key] = value;
    }

    public string GetSetting(string key)
    {
        return _settings.TryGetValue(key, out var value) ? value : "Setting not found";
    }
}

/*
* 1.Singleton Implementation:

    private static readonly ConfigurationManager _instance = new ConfigurationManager(); creates the single instance of ConfigurationManager.

    public static ConfigurationManager Instance => _instance; provides global access to this instance.

    The constructor is private to restrict instantiation to within the class.

* 2.Configuration Methods:

    SetSetting uses the dictionary to store or update configuration settings.

    GetSetting retrieves the setting for the specified key or returns "Setting not found" if the key doesn’t exist.
 
 */