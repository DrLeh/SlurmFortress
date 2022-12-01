namespace SlurmFortress.Algolia.Indexing.Seeding;

public sealed class AlgoliaSeedOptions
{
    public IndexType Types { get; set; } = IndexType.All;
    public int StartPage { get; set; }
    public int PageSize { get; set; }
    public long TenantId { get; set; }
}
