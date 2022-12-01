using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SlurmFortress.Data.Mappings;

public sealed class SlurmMapping : EntityMapping<Slurm>
{
    protected override void ConfigureInternal(EntityTypeBuilder<Slurm> b)
    {
        //b.Property(x => x.Name).HasMaxLength(128);
    }
}
