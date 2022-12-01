//using System.Net;
//using System.Reflection;
//using ASI.Console;
//using ASI.Console.Commands;
//using ASI.Services.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using SlurmFortress.Console.Infrastructure;


//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
////logging in .net core is odd, seems to require this host building
////https://www.thecodebuzz.com/logging-using-log4net-net-core-console-application/

//var builder = new HostBuilder()
//    .ConfigureServices((hostContext, services) =>
//    {
//        //add all the ConsoleCommands to the registry
//        services.RegisterAllTypes<IAsiConsoleCommandBase>(new[]
//        {
//            Assembly.GetExecutingAssembly()!,
//            Assembly.GetAssembly(typeof(AsiConsole))!
//        });

//        services.AddSingleton<IEnvironmentFactory<SlurmFortressEnvironment>, SlurmFortressEnvironmentFactory>();
//        services.AddSingleton<AsiConsoleAsync<SlurmFortressEnvironment>>();
//    })
//    .ConfigureLogging(logBuilder =>
//    {
//        logBuilder.AddLog4Net();
//    })
//    .UseConsoleLifetime();

//var host = builder.Build();
//using (var scope = host.Services.CreateScope())
//{
//    var console = scope.ServiceProvider.GetService<AsiConsoleAsync<SlurmFortressEnvironment>>()!;
//    await console.StartAsync(args);
//}
Console.WriteLine("hi");