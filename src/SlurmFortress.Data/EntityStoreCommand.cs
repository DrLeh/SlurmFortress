namespace SlurmFortress.Data;

//here you'd implement this interface, and configure how those commands get executed
// be it via Entity Framework, or what have you
public sealed class EntityStoreCommand<TModel> : IEntityStoreCommand
    where TModel : IEntity
{
    public StoreInteractionType Type { get; }
    public object Entity { get; }

    public EntityStoreCommand(StoreInteractionType action, IEntity model)
    {
        Type = action;
        Entity = model ?? throw new ArgumentNullException(nameof(model));
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
