using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SlurmFortress.Data.Mappings;

public abstract class EntityMapping<T> : IEntityTypeConfiguration<T>
    where T : class, IEntity
{
    public virtual void Configure(EntityTypeBuilder<T> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.CreatedBy).HasMaxLength(EntityMetadata.UserMaxLength).HasDefaultValueSql("((SUSER_SNAME()+' Proc=')+ISNULL(OBJECT_NAME(@@procid),''))");
        b.Property(x => x.UpdatedBy).HasMaxLength(EntityMetadata.UserMaxLength);

        b.Property(x => x.CreateDate).HasColumnName("CreateDateUTC").HasDefaultValueSql("GETUTCDATE()");
        b.Property(x => x.UpdateDate).HasColumnName("UpdateDateUTC");

        ConfigureInternal(b);
    }

    protected abstract void ConfigureInternal(EntityTypeBuilder<T> b);
}        
