namespace SlurmFortress.Algolia.Indexing.Seeding;

[Flags]
public enum IndexType
{
    None = 0,
    All = ~0,
    Slurm = 0b1,
}
