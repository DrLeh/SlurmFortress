using System.Linq.Expressions;

namespace SlurmFortress.Data;

public sealed class DataTransaction : IDataTransaction
{
    public IReadOnlyList<IStoreCommand> Commands => _commands;
    private readonly DataAccess _dataAccess;
    private readonly List<IStoreCommand> _commands;
    private readonly List<Action> _beforeCommit;
    private readonly List<Action> _afterCommit;

    public DataTransaction(DataAccess dataAccess)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        _commands = new List<IStoreCommand>();
        _beforeCommit = new List<Action>();
        _afterCommit = new List<Action>();
    }

    public void Add<T>(T entity)
        where T : IEntity
    {
        _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Added, entity));
    }

    public void Update<T>(T entity)
        where T : IEntity
    {
        _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Updated, entity));
    }

    public void Remove<T>(T entity)
        where T : IEntity
    {
        _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Removed, entity));
    }

    public void BulkRemove<T>(Expression<Func<T, bool>> selector)
        where T : class, IEntity
    {
        //_commands.Add(new BulkRemoveCommand<T>(selector));
    }

    public void BulkUpdate<T>(Expression<Func<T, bool>> selector, Expression<Func<T, object>> obj)
        where T : class, IEntity
    {
        //_commands.Add(new BulkUpdateCommand<T>(selector, obj));
    }

    public void SoftDelete<T>(Expression<Func<T, bool>> selector)
        where T : class, IEntity
    {
        //_commands.Add(new BulkUpdateCommand<T>(selector, x => new { IsDeleted = true }));
    }

    public void Commit()
    {
        _dataAccess.ExecuteCommands(_commands);
        _commands.Clear();
    }

    public async Task CommitAsync()
    {
        await _dataAccess.ExecuteCommandsAsync(_commands);
        _commands.Clear();
    }
}
