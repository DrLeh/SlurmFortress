using ASI.Services.Access;
using SlurmFortress.Core.Context;

namespace ASI.Barista.Plugins.SlurmFortress.Events;

public class ConsumerUserInformation : IUserInformation
{
    public ConsumerUserInformation(long tenantId, long userId)
    {
        TenantId = tenantId;
        UserId = userId;
    }

    public long TenantId { get; set; }
    public long UserId { get; }
    public long OwnerId { get => UserId; set { } }

    public string UserName => "SlurmFortress Event Consumer";

    public string? IpAddress { get; set; }
    public Guid? SessionId => null;

    public List<UserPermission> Permissions { get; set; } = null!;//todo: fill in all permissions from this

    //might need a new security policy for the consumer
    public List<long> Teams => new List<long>();

    public bool IsAnonymousUser => false;


    public string PrimaryEmail => throw new NotImplementedException();

    public string EmailHash => throw new NotImplementedException();

    IReadOnlyList<long> ICurrentUser.Teams => throw new NotImplementedException();

    long ITenant.OwnerId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

