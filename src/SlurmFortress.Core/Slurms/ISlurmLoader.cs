using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Slurms;

public interface ISlurmLoader
{
    Slurm? Get(Guid id);
}
