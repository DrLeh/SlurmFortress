using ASI.Services.Algolia.Client;
using ASI.Services.Search.Indexing;
using Microsoft.Extensions.Logging;
using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Algolia.Indexing.Seeding;

/// <summary>
/// Example seeder implementation
/// </summary>
public sealed class SlurmSeeder : SeederBase<Slurm>
{
    public override IndexType Type => IndexType.Slurm;

    public SlurmSeeder(IDataAccess dataAccess, ISearchIndexProvider algoliaIndexProvider,
        ISearchTransactionProvider searchTransactionProvider,
        ISearchIndexNameProvider searchIndexNameProvider, ILoggerFactory loggerFactory)
        : base(dataAccess, algoliaIndexProvider, searchTransactionProvider, searchIndexNameProvider, loggerFactory)
    {
    }

    public override IQueryable<Slurm> BaseQuery(AlgoliaSeedOptions options)
    {
        return _dataAccess.Query<Slurm>()
            .Where(x => x.IsDeleted == false
                && (options.TenantId == 0 || x.TenantId == options.TenantId));
    }

    public override IQueryable<Slurm> LoadQuery(int page, AlgoliaSeedOptions options)
    {
        return _dataAccess.Query<Slurm>(/* Include joins here*/)
            .Where(x => x.IsDeleted == false
                && (options.TenantId == 0 || x.TenantId == options.TenantId))
            .OrderByDescending(x => x.Id);
    }

    public override void PostLoadStep(List<Slurm> records)
    {
        foreach (var record in records)
        {
            //go to an external service here to populate additional data if needed
            //_collaboratorLoader.PopulateCollaboratorAsync(record).GetAwaiter().GetResult();
        }
    }
}
