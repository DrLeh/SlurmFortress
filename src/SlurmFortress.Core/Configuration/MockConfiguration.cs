namespace SlurmFortress.Core.Configuration;

//Useful for unit tests 
public class MockConfiguration : IConfiguration
{
    public string? EnvironmentName { get; set; }
    public string ConnectionString { get; set; } = "";
    public string EsbConnectionString { get; set; } = "";
}
