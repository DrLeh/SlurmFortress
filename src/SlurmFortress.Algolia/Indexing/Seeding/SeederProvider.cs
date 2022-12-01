namespace SlurmFortress.Algolia.Indexing.Seeding;

public sealed class SeederProvider
{
    public Dictionary<IndexType, ISeeder> Seeders { get; }
    public ISeeder GetSeeder(IndexType type) => Seeders[type];

    public SeederProvider(IEnumerable<ISeeder> seeders)
    {
        if (seeders == null)
            throw new ArgumentNullException(nameof(seeders));

        Seeders = seeders.ToDictionary(x => x.Type, x => x);
    }
}
