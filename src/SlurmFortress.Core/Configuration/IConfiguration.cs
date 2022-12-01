namespace SlurmFortress.Core.Configuration;

public interface IConfiguration
{
    string? EnvironmentName { get; }
    string ConnectionString { get; }
    string EsbConnectionString { get; }
}    
