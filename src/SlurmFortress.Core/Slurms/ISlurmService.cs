using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Slurms;

public interface ISlurmService
{
    Task<Slurm> AddAsync(Slurm toAdd);
    Slurm? Get(Guid id);
    Task<Slurm> UpdateAsync(Guid id, Action<Slurm> update);
}
