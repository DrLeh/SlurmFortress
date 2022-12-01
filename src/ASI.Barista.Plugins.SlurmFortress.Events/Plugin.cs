using ASI.Barista.Plugins.Messaging;
using ASI.Barista.Plugins.SlurmFortress.Events.Consumers;
using ASI.Barista.Plugins.Ipc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ASI.Barista.Plugins.SlurmFortress.Events;

public class Plugin : PluginBase
{
    public Plugin(IServiceCollection services, IConfiguration config, IHostEnvironment host)
        : base(services, config, host)
    {
        StartPlugin(services, config);
    }

    private void StartPlugin(IServiceCollection services, IConfiguration? config)
    {
        //build a service provider for resolving the consumers only.
        // The consumer itself will create a new service provider for each received message
        var sp = DependencyInjection.BuildConsumerServiceProvider(services, null, config);

        //configure this plugin
        Configure()
            .WithEventLogging()
            .AddConsumer(() => sp.GetRequiredService<SlurmFortressEventConsumer>(), c => c
                .AddTopic($"myentity.added")
                .AddTopic($"myentity.updated")
            )
            .OnStart(() =>
            {
                Logger?.LogInformation($"################# Starting plugin ASI.Barista.Plugins.SlurmFortress.Events");
            })
            .CleanupEsbOnShutdown()
            .WithInterprocessCommunication()
            ;
    }
}
