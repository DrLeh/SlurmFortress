using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Helpers.Exceptions;

public static class EntityExceptionsExtensions
{
    public static T ValidateFound<T>(this T? entity, Guid id)
        where T : class, IEntity
    {
        if (entity == null)
            throw new EntityNotFoundException(id, typeof(T));
        return entity;
    }
}
