using System.Linq.Expressions;

namespace SlurmFortress.Core.Data;

public interface IBulkStoreCommand : IStoreCommand
{

}

public interface IBulkStoreCommand<T> : IBulkStoreCommand
{
    Expression<Func<T, bool>> Selector { get; }
}
