using log4net;
using System.Linq.Expressions;
using System.Reflection;

namespace SlurmFortress.Data.Context;

public sealed class SlurmFortressDbContext : DbContext, IDbContext
{
    private readonly static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
    private readonly Core.Configuration.IConfiguration _configuration;
    private readonly IChangeTrackerManager? _changeTrackerManager;

    private bool IsInMemoryDb => Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
    public bool SupportsJson() => !IsInMemoryDb && _configuration.EnvironmentName != "DEV";
    public bool SupportsBulk() => !IsInMemoryDb;

    public SlurmFortressDbContext(DbContextOptions<SlurmFortressDbContext> options,
        Core.Configuration.IConfiguration configuration, IChangeTrackerManager? changeTrackerManager)
        : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _changeTrackerManager = changeTrackerManager;
        //this.ChangeTracker.AutoDetectChangesEnabled = false;
    }

#if DEBUG
    bool DEBUG = true;
#else
        bool DEBUG = false;
#endif

    public override int SaveChanges()
    {
        _changeTrackerManager?.FixupEntities(this);
        try
        {
            return base.SaveChanges();
        }
        catch (Exception e) when (DEBUG)
        {
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _changeTrackerManager?.FixupEntities(this);

        try
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e) when (DEBUG)
        {
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SlurmFortressDbContext))!);
    }

    void IDbContext.Add(object entity)
    {
        base.Add(entity);
    }

    void IDbContext.Update(object entity)
    {
        base.Update(entity);
    }

    void IDbContext.Remove(object entity)
    {
        base.Remove(entity);
    }

    public IQueryable<T> Query<T>(Expression<Func<T, bool>> filter)
        where T : class, IEntity
    {
        return base.Set<T>().Where(filter).AsQueryable();
    }

    public string GetTableName<T>()
        where T : class, IEntity
    {
        return base.Model.FindEntityType(typeof(T))?.GetTableName() ?? "No table found";
    }

    public void Execute(FormattableString sql)
    {
        //not supported by in-memory database
        if (!IsInMemoryDb)
        {
            var rawSql = string.Format(sql.Format, sql.GetArguments());
            base.Database.ExecuteSqlRaw(rawSql);
        }
        //base.Database.ExecuteSqlInterpolated(sql); //throws some @p0 exception - DJL 2/4/2020
    }
}