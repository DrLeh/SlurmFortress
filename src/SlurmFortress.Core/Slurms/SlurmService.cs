using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;
using SlurmFortress.Core.Slurms.Operations;

namespace SlurmFortress.Core.Slurms;

public sealed class SlurmService : ISlurmService
{
    private readonly IDataAccess _dataAccess;
    private readonly ISlurmLoader _loader;

    public SlurmService(IDataAccess dataAccess,
        ISlurmLoader loader)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        _loader = loader ?? throw new ArgumentNullException(nameof(loader));
    }

    public Slurm? Get(Guid id)
    {
        return _loader.Get(id);
    }


    /// <summary>
    /// Simple scenario that doesn't really need unit testing
    /// </summary>
    public async Task<Slurm> AddAsync(Slurm toAdd)
    {
        var trans = _dataAccess.CreateTransaction();

        trans.Add(toAdd);

        await trans.CommitAsync();

        return toAdd;
    }

    /// <summary>
    /// More complex scenario
    /// </summary>
    public async Task<Slurm> UpdateAsync(Guid id, Action<Slurm> update)
    {
        var trans = _dataAccess.CreateTransaction();
        var context = new SlurmUpdateContext(id, update, _loader, trans);
        var op = new SlurmUpdateOperation(context);

        op.Load();
        op.Validate();
        op.StageChanges();

        await trans.CommitAsync();

        return context.Entity!;
    }
}
