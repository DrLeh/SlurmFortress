namespace SlurmFortress.Core.Data;

public interface IStoreCommand
{
    StoreInteractionType Type { get; }
    void Execute(IDbContext context);
}
