using ASI.Services.Algolia.Client;
using SlurmFortress.Core.Search.Models;

namespace SlurmFortress.Algolia.Indexing;

public class SlurmIndexDefinition : DefaultIndexDefinition<SearchSlurm>
{
    protected override void ConfigureIndex_Internal(IIndexSettingsDescriptor<SearchSlurm> builder)
    {
        builder.Ranking(x => x.Descending(x => x.UpdateDate!))
            .AttributesForFaceting(x => x
                .Searchable(y => y.TenantId)
                .NotSearchable(y => y.Id)
                )
            .SearchableAttributes(s => s
                .Ordered(o => o.Name)
                .Ordered(o => o.Description!))
            .Replicas(r => r
                .AddReplica("az", builder, rs =>
                    rs.Ranking(y => y.None().Ascending(x => x.Name))
                )
                .AddReplica("za", builder, rs =>
                    rs.Ranking(y => y.None().Descending(x => x.Name))
                ))
            ;
    }
}
