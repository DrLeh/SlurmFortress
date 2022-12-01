using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SlurmFortress.Data.Context;

public interface IChangeTrackerManager
{
    void FixupEntities(DbContext dbContext);
}

/// <summary>
/// To be used when you do need to actually make changes to `NeverCommit` objects, etc
/// </summary>
public class NullChangeTrackerManager : IChangeTrackerManager
{
    public void FixupEntities(DbContext dbContext)
    {
        // do nothing
    }
}

/// <summary>
/// Fix up audit fields, truncate text, any other pre-processing to be done before saving to the database.
/// </summary>
public class ChangeTrackerManager : IChangeTrackerManager
{
    private readonly IAuditFieldFixer? _auditFieldFixer;
    private readonly ILogger<ChangeTrackerManager>? _logger;

    public ChangeTrackerManager(IAuditFieldFixer? auditFieldFixer, ILoggerFactory loggerFactory)
    {
        _auditFieldFixer = auditFieldFixer;
        _logger= loggerFactory?.CreateLogger<ChangeTrackerManager>();
    }

    public void FixupEntities(DbContext dbContext)
    {
        var entries = dbContext.ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            var ignoreAtt = entry.Entity.GetType().GetCustomAttribute<NeverCommitAttribute>();
            if (ignoreAtt is not null)
            {
                entry.State = EntityState.Unchanged;
            }
            else if (typeof(ILookup).IsAssignableFrom(entry.Entity.GetType()))
            {
                entry.State = EntityState.Unchanged;
                //attach it instead
                dbContext.Attach(entry.Entity);
            }
        }

        var auditableEntries = dbContext.ChangeTracker.Entries<IAuditable>();
        foreach (var entry in auditableEntries)
        {
            _auditFieldFixer?.FixAuditFields(entry.Entity, GetEntityState(entry));
        }

        TruncateStringForChangedEntities(dbContext);
        FixupUTCDates(dbContext);
    }

    private DbEntityState GetEntityState<T>(EntityEntry<T> entry)
        where T : class
    {
        switch (entry.State)
        {
            case EntityState.Added:
                return DbEntityState.Added;
            case EntityState.Modified:
                return DbEntityState.Modified;
            default:
                return DbEntityState.Unknown;
        }
    }

    /// <summary>
    /// Postgres requires you to have datetime kind set to UTC. And we should be using every date as UTC anyway
    /// </summary>
    public void FixupUTCDates(DbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
    }

    //https://devhow.net/2019/01/17/entity-framework-core-truncating-strings-based-on-length-constraint/
    public void TruncateStringForChangedEntities(DbContext context)
    {
        var stringPropertiesWithLengthLimitations = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(string))
            .Select(z => new
            {
                StringLength = z.GetMaxLength(),
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            })
            .Where(d => d.StringLength.HasValue);


        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = stringPropertiesWithLengthLimitations.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as string;
                if (originalValue == null)
                    continue;

                if (originalValue.Length > property.StringLength)
                {
                    var entityTyped = entity as IEntity;
                    _logger?.LogDebug($"Entity '{entity.GetType().Name}':{entityTyped?.Id} Had value truncated from {originalValue.Length} to {property.StringLength} on property '{property.PropertyName}'");
                    prop.SetValue(entity, originalValue.Substring(0, property.StringLength.Value));
                }
            }
        }
    }

}
