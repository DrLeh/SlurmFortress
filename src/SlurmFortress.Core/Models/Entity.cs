namespace SlurmFortress.Core.Models;

public abstract class Entity : IEntity
{
    /// <summary>
    /// Primary Key
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Denotes that the entity is soft-deleted
    /// </summary>
    public bool IsDeleted { get; set; }
    public DateTime CreateDate { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? UpdateDate { get; set; }
    public string? UpdatedBy { get; set; }
}        
