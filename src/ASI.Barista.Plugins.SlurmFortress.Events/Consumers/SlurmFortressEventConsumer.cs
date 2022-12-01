using ASI.Contracts.SlurmFortress.Messages;
using ASI.Services.Logging;
using ASI.Services.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SlurmFortress.Core.Slurms;

namespace ASI.Barista.Plugins.SlurmFortress.Events.Consumers;

public class SlurmFortressEventConsumer : IConsumer<SlurmFortressEvent>
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<SlurmFortressEventConsumer> _logger;
    public Configuration Configuration { get; }
    public string Id { get; set; } = "myapp_event_consumer";

    public SlurmFortressEventConsumer(Configuration esbConfig, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        Configuration = esbConfig;
        _loggerFactory = loggerFactory;
        _configuration = configuration;
        _logger = loggerFactory.CreateLogger<SlurmFortressEventConsumer>();
        _logger.LogInformation($"Starting {nameof(SlurmFortressEventConsumer)}");
    }

    public void Consume(SlurmFortressEvent message)
    {
        try
        {
            _logger.LogInformation($"Received {nameof(SlurmFortressEvent)} Id={message.Header.Id} Event={message.Event}");
            //example of how to handle this event differently
            Action<SlurmFortressEvent> action = message.Event.ToLower() switch
            {
                SlurmFortressEvent.Added => SlurmAdded,
                SlurmFortressEvent.Updated => SlurmUpdated,
                SlurmFortressEvent.Removed => SlurmRemoved,
                _ => NotSupported
            };
            action(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error in {Id}: {e.Message}");
            throw;
        }
    }

    private void NotSupported(SlurmFortressEvent message)
    {
        //throw new InvalidOperationException($"Message type {message.Event} not supported");
    }

    private void SlurmAdded(SlurmFortressEvent message)
    {
        _logger.LogInformation($"MsgId={message.Header.Id} Entity Added Id={message.EntityId}");
        //within a consumer, build a service provider for this specific message
        var sp = message.GetServiceProvider(_loggerFactory, _configuration);
        //now we resolve our service and call the change
        var svc = sp.GetRequiredService<ISlurmService>();
        //svc.TrackAddEvent();
    }

    private void SlurmUpdated(SlurmFortressEvent message)
    {
        // todo
    }
    private void SlurmRemoved(SlurmFortressEvent message)
    {
        // todo
    }
}
