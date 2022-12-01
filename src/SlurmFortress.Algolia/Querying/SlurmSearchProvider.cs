using Algolia.Search.Models.Search;
using ASI.Services.Access;
using ASI.Services.Algolia.Client;
using ASI.Services.Search.Models;
using Microsoft.Extensions.Logging;
using SlurmFortress.Core.Search.Models;
using SlurmFortress.Core.Search.Providers;

namespace SlurmFortress.Algolia.Querying;

public sealed class SlurmSearchProvider : SecureSearchProviderBase<SearchSlurm>, ISlurmSearchProvider
{
    private readonly ICurrentUser _currentUser;

    public SlurmSearchProvider(ISearchIndexProvider indexProvider,
        ISecureAlgoliaIndexProvider secureAlgoliaIndexProvider,
        ICurrentUser currentUser,
        ILoggerFactory loggerFactory)
        : base(indexProvider, secureAlgoliaIndexProvider, loggerFactory)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public override SearchResult<SearchSlurm> TransformResult(SearchRequest request, SearchResponse<SearchSlurm> searchResponse)
    {
        var result = new SearchResult<SearchSlurm>
        {
            ResultsTotal = searchResponse.NbHits,
            Results = searchResponse.Hits
                .ToList(),
        };

        return result;
    }

    protected override Query BuildSearchQuery(SearchRequest criteria)
    {
        var builder = new SlurmSearchQueryBuilder()
            .WithType(criteria.Type, _currentUser.OwnerId)
            ;
        return builder.Build(criteria);
    }
}
