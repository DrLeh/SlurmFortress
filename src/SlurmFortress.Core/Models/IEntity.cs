namespace SlurmFortress.Core.Models;

public interface IAuditable
{
    string CreatedBy { get; set; }
    DateTime CreateDate { get; set; }

    string? UpdatedBy { get; set; }
    DateTime? UpdateDate { get; set; }
}

public interface IEntity : IAuditable
{
    /// <summary>
    /// Primary Key
    /// </summary>
    Guid Id { get; set; }

    bool IsDeleted { get; set; }
}


public interface ILookup : IAuditable
{
    /// <summary>
    /// Primary Key
    /// </summary>
    string Code { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}