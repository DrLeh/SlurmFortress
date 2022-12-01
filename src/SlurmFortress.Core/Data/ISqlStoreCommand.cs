using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Data;

public interface ISqlStoreCommand<T> : IStoreCommand
    where T : class, IEntity
{

}
