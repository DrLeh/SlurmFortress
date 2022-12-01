using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;
using System.Linq.Expressions;

namespace SlurmFortress.Core.Test;

public class TestStoreCommand<T> : IEntityStoreCommand<T>
    where T : IEntity
{
    public StoreInteractionType Type { get; }
    public object Entity { get; }
    public T EntityTyped { get; }

    public TestStoreCommand(StoreInteractionType type, T entity)
    {
        Type = type;
        Entity = entity;
        EntityTyped = entity;
    }

    public void Execute(IDbContext context)
    {
        switch (Type)
        {
            case StoreInteractionType.Added: context.Add(Entity); break;
            case StoreInteractionType.Updated: context.Update(Entity); break;
            case StoreInteractionType.Removed: context.Remove(Entity); break;
        }
    }
}
public class TestBulkStoreCommand<T> : IBulkStoreCommand<T>
    where T : IEntity
{
    public Expression<Func<T, bool>> Selector => throw new NotImplementedException();
    public StoreInteractionType Type { get; }

    public TestBulkStoreCommand(StoreInteractionType type)
    {
        Type = type;
    }


    public void Execute(IDbContext context)
    {

    }
}
