using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Slurms;

//In orders code, the implementation of this loader had to be in the Data project
//however, with the way we've abstracted our IDataAccess, it's not necessary to use
// EF-specific code in the loaders.
// Of course, it's always an option to put the loader into Data project,
// but in many cases even the loaders should strive to not be dependent on EF-specific functions
public sealed class SlurmLoader : ISlurmLoader
{
    private readonly IDataAccess _dataAccess;

    public SlurmLoader(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
    }

    public Slurm? Get(Guid id)
    {
        return _dataAccess.Query<Slurm>()
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }
}
