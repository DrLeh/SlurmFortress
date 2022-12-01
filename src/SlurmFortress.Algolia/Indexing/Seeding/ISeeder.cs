using SlurmFortress.Core.Models;

namespace SlurmFortress.Algolia.Indexing.Seeding;

public interface ISeeder
{
    IndexType Type { get; }
    void Seed(AlgoliaSeedOptions options);
    int PageSize { get; }
}

public interface ISeeder<T> : ISeeder
    where T : class, IEntity
{
}
