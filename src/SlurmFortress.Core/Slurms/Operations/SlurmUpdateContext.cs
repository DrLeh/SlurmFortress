using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Slurms.Operations;

public sealed class SlurmUpdateContext
{
    public Guid Id { get; }
    public Action<Slurm> UpdateFunc { get; }
    public ISlurmLoader Loader { get; }
    public IDataTransaction DataTransaction { get; }
    public Slurm? Entity { get; set; }

    public SlurmUpdateContext(
        Guid id,
        Action<Slurm> updateFunc,
        ISlurmLoader loader,
        IDataTransaction dataTransaction
        )
    {
        Id = id;
        UpdateFunc = updateFunc ?? throw new ArgumentNullException(nameof(updateFunc));
        Loader = loader ?? throw new ArgumentNullException(nameof(loader));
        DataTransaction = dataTransaction ?? throw new ArgumentNullException(nameof(dataTransaction));
    }
}
