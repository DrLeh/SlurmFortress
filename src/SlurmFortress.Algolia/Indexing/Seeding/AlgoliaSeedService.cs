using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SlurmFortress.Algolia.Indexing.Seeding;

public sealed class AlgoliaSeedService
{
    private readonly SeederProvider _seederProvider;
    private readonly ILogger<AlgoliaSeedService> _logger;

    public AlgoliaSeedService(SeederProvider seederProvider, ILoggerFactory loggerFactory)
    {
        if (loggerFactory == null)
            throw new ArgumentNullException(nameof(loggerFactory));

        _seederProvider = seederProvider ?? throw new ArgumentNullException(nameof(seederProvider));
        _logger = loggerFactory.CreateLogger<AlgoliaSeedService>();
    }

    public void RunSeed(AlgoliaSeedOptions options)
    {
        if (options.Types == IndexType.None)
        {
            _logger.LogInformation("No types provided, done");
            return;
        }

        foreach (IndexType type in Enum.GetValues(options.Types.GetType()))
            if (options.Types.HasFlag(type) && type != IndexType.None && type != IndexType.All)
                Process(type, options);
    }

    public void Process(IndexType type, AlgoliaSeedOptions options)
    {
        _logger.LogInformation($"Beginnging seed {type}");
        var stop = Stopwatch.StartNew();

        var seeder = _seederProvider.GetSeeder(type);
        seeder.Seed(options);

        stop.Stop();
        _logger.LogInformation($"Finished seed of {type} in {stop.Elapsed}");
    }
}
