using SlurmFortress.Core.Context;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Data;

//Needs to be named differently from EntityState, which is defined in EF core
public enum DbEntityState
{
    Unknown,
    Added,
    Modified,
}

/// <summary>
/// Fixup data for all entities before committing to database
/// </summary>
public interface IAuditFieldFixer
{
    void FixAuditFields(IAuditable entity, DbEntityState state);
}

/// <summary>
/// Fixup data for all entities before committing to database
/// </summary>
public class AuditFieldFixer : IAuditFieldFixer
{
    private readonly IUserInformation _userInformation;

    public AuditFieldFixer(IUserInformation userInformation)
    {
        _userInformation = userInformation;
    }

    public void FixAuditFields(IAuditable entity, DbEntityState state)
    {
        if (state == DbEntityState.Added)
        {
            entity.CreateDate = DateTime.UtcNow;
            entity.CreatedBy = _userInformation?.UserName ?? string.Empty;
        }
        else if (state == DbEntityState.Modified)
        {
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedBy = _userInformation?.UserName;
        }
    }
}
