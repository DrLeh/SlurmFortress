using ASI.Services.Algolia.Client;
using SlurmFortress.Core.Configuration;
using SlurmFortress.Core.Search.Models;

namespace SlurmFortress.Algolia;

public class AlgoliaIndexNameProvider : DictionaryIndexNameProvider
{
    private readonly IConfiguration _config;

    public AlgoliaIndexNameProvider(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public override Dictionary<Type, string> IndexNames => new Dictionary<Type, string>
    {
        [typeof(SearchSlurm)] = "myEntity",
    };

    public override bool FallbackToTypeName => false;

    public override string GetIndexName<T>()
    {
        return $"{_config.EnvironmentName?.ToLower() ?? "local"}_{base.GetIndexName<T>()}";
    }
}
