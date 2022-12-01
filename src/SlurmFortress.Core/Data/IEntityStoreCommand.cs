namespace SlurmFortress.Core.Data;

public interface IEntityStoreCommand : IStoreCommand
{
    object Entity { get; }
}

public interface IEntityStoreCommand<out T> : IEntityStoreCommand
{
    T EntityTyped { get; }
}
