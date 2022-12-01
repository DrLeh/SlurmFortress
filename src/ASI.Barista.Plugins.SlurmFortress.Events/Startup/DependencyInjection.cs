using ASI.Barista.Plugins.SlurmFortress.Events.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SlurmFortress.Algolia;
using SlurmFortress.Core.Context;
using SlurmFortress.Core;
using SlurmFortress.Data;
using ASI.Contracts.SlurmFortress.Messages;
using ASI.Services.Logging;
using ASI.Services.Messaging;

namespace ASI.Barista.Plugins.SlurmFortress.Events;

public static class DependencyInjection
{
    /// <summary>
    /// Gets a service provider "scoped" to the details provided in the message
    /// </summary>
    public static IServiceProvider GetServiceProvider(this SlurmFortressEvent message, ILoggerFactory? loggerFactory, IConfiguration? configuration)
    {
        return GetServiceProvider(message.TenantId, message.UserId, loggerFactory, configuration);
    }

    public static IServiceProvider GetServiceProvider(long tenantId, long userId, ILoggerFactory? loggerFactory, IConfiguration? configuration)
    {
        var services = ConfigureServices(loggerFactory, configuration);
        RegisterUser(services, tenantId, userId);
        return services.BuildServiceProvider();
    }

    public static IServiceCollection ConfigureServices(ILoggerFactory? loggerFactory, IConfiguration? configuration)
    {
        var services = new ServiceCollection();
        AddBase(services, loggerFactory, configuration);

        services.AddCore();
        services.AddData();
        services.AddAlgolia();

        return services;
    }

    /// <summary>
    /// Register all things pertaining to the current user
    /// </summary>
    private static void RegisterUser(IServiceCollection services, long tenantId, long userId)
    {
        services.AddSingleton<IUserInformation>(sp => new ConsumerUserInformation(tenantId, userId));
    }

    public static IServiceProvider BuildConsumerServiceProvider(IServiceCollection services, ILoggerFactory? loggerFactory, IConfiguration? configuration)
    {
        services.AddSingleton<SlurmFortressEventConsumer>();
        AddBase(services, loggerFactory, configuration);
        return services.BuildServiceProvider();
    }

    private static void AddBase(IServiceCollection services, ILoggerFactory? loggerFactory, IConfiguration? configuration)
    {
        //when running locally, we don't have the config injected by Barista automatically.
        // therefore, create it manually here.
        if (configuration == null)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(PluginEnvironment.CurrentDirectory)
                .AddJsonFile("plugin.config.json", true)
                .Build();
            services.AddSingleton(configuration);
        }
        else
        {
            services.AddSingleton(configuration);
        }

        //same here, we don't have the ILoggerFactory injected when running in local,
        // so use the normal built-in logging here.
        if (loggerFactory == null)
        {
            services.AddLogging();
        }
        else
        {
            services.AddSingleton(loggerFactory);
        }
        loggerFactory?.AddLog4Net(configuration);
        services.AddEsb();

        //if you need to connect to another service, use these to register the token provider
        //services.AddAsiLogin();
        //services.AddSingleton<IAsiTokenProvider, ConsumerTokenProvider>();
    }
}

