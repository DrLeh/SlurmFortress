using SlurmFortress.Data.Context;
using System.Linq.Expressions;

namespace SlurmFortress.Data;

public class DataAccess : IDataAccess
{
    private readonly SlurmFortressDbContext _dbContext;

    public DataAccess(SlurmFortressDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IDataTransaction CreateTransaction()
    {
        return new DataTransaction(this);
    }

    private IQueryable<T> Query_Internal<T>() where T : class, IEntity
    {
        var q = _dbContext.Set<T>().AsQueryable();

        q = q.Where(x => !x.IsDeleted);

        return q;
    }

    private IQueryable<T> Query_Internal<T>(params Expression<Func<T, object>>[] includes) where T : class, IEntity
    {
        var q = Query_Internal<T>();

        foreach (var i in includes)
            q = q.Include(i);

        return q;
    }

    public IQueryable<T> Query<T>() where T : class, IEntity
    {
        return Query_Internal<T>();
    }

    public IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class, IEntity
    {
        return Query_Internal<T>(includes);
    }

    public IQueryable<T> Include<T, TP1>(IQueryable<T> query, Expression<Func<T, TP1>> expr1) where T : class
    {
        return query.Include(expr1);
    }

    public IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, TP1>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class
    {
        return query.Include(expr1).ThenInclude(expr2);
    }

    public IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, ICollection<TP1>>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class
    {
        return query.Include(expr1).ThenInclude(expr2);
    }

    public void ExecuteCommands(IReadOnlyList<IStoreCommand> commands)
    {
        Execute_Internal(commands);

        _dbContext.SaveChanges();
    }

    public Task ExecuteCommandsAsync(IReadOnlyList<IStoreCommand> commands)
    {
        Execute_Internal(commands);

        return _dbContext.SaveChangesAsync();
    }

    private void Execute_Internal(IReadOnlyList<IStoreCommand> commands)
    {
        foreach (var command in commands)
        {
            switch (command)
            {
                case IEntityStoreCommand entityCmd:

                    break;
            }

            command.Execute(_dbContext);
        }
    }
}
