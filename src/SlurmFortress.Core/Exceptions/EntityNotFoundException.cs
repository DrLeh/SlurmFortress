using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Helpers.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Guid id, IEntity entity) : base($"{entity.GetType().Name}: {id} was not found")
    {
    }

    public EntityNotFoundException(Guid id, Type entity) : base($"{entity.Name}: {id} was not found")
    {
    }
}
