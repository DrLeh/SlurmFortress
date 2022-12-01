using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SlurmFortress.Core.Configuration;

namespace SlurmFortress.Data.Context;

public sealed class SlurmFortressDbContextFactory : IDesignTimeDbContextFactory<SlurmFortressDbContext>
{
    public IConfigurationRoot _config;
    public string _clientCertPath = "";
    public string _connString = "";

    public SlurmFortressDbContextFactory()
    {
        var env = "UAT";

        _config = new ConfigurationBuilder()
            .AddJsonFile("dbsettings.json")
            .Build();
        _connString = _config.GetConnectionString("ZealConnection_" + env);

        _clientCertPath = "";
    }

    /// <summary>
    /// this is the endpoint used in Package Manager console. Change this to update a different database
    /// </summary>
    public SlurmFortressDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SlurmFortressDbContext>();
        //optionsBuilder.UseSqlServer(_connString); //if using sql
        optionsBuilder.UseNpgsql(_connString, x =>
        {
        });
        return new SlurmFortressDbContext(optionsBuilder.Options, new MockConfiguration { EnvironmentName = "DEV" }, null!);
    }
}
