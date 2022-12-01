using Microsoft.Extensions.Configuration;

namespace SlurmFortress.Web.Api.Test.Tests;

public class FileConfiguration : Core.Configuration.IConfiguration
{
    protected IConfigurationRoot _config;

    public FileConfiguration()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public string SystemUnderTest => _config[nameof(SystemUnderTest)];

    public string? EnvironmentName => _config[nameof(EnvironmentName)];
    public string ConnectionString => _config.GetConnectionString("SlurmFortressDatabase");
    public string EsbConnectionString => _config["EsbConnectionString"];

    public bool SUTIsLocal => string.IsNullOrWhiteSpace(SystemUnderTest) || SystemUnderTest.ToLowerInvariant() == "local";
}
