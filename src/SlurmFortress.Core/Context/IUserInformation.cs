
namespace SlurmFortress.Core.Context;

/// <summary>
/// Represents information from the current authenticated user
/// </summary>
public interface IUserInformation
{
    long UserId { get; }
    string UserName { get; }
}


/// <summary>
/// single user for debug testing
/// </summary>
public class TestUserInformation : IUserInformation
{
    public long UserId => 1;
    public string UserName => "Devon";
}