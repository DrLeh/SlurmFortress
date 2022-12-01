using ASI.Services.Algolia.Search;
using SlurmFortress.Core.Search.Models;

namespace SlurmFortress.Algolia.Querying;

public sealed class SlurmSearchQueryBuilder : SearchQueryBuilderBase<SearchSlurm, SlurmSearchQueryBuilder>
{
    public SlurmSearchQueryBuilder WithType(string? type, long ownerId)
    {
        if (string.IsNullOrWhiteSpace(type))
            return this;

        if (type.ToLowerInvariant() == "me")
        {
            Descriptor.Filters(x => x.OwnerId == ownerId);
        }
        else if (type.ToLowerInvariant() == "shared")
        {
            Descriptor.Filters(x => x.OwnerId != ownerId);
        }

        return this;
    }
}
