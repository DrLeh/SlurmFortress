using Microsoft.Extensions.Configuration;

namespace ASI.Barista.Plugins.SlurmFortress.Events;

public static class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json")
            .Build()
            ;
        var plugin = new Plugin(new ServiceCollection(), config, null!);

        try
        {
            plugin.Start();
            new ManualResetEvent(false).WaitOne(Timeout.Infinite);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.ReadLine();
        }
        finally
        {
            Console.WriteLine("Ending Plugin " + plugin.Name);
            plugin.Stop();
            plugin.Dispose();
        }
    }
}
