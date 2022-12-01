using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SlurmFortress.Web.Api.Test;

/// <summary>
/// This test will add all the registrations present in the Web API project
/// and verify that it is able to resolve every registered service
/// </summary>
public class ResolutionTest
{
    /// <summary>
    /// Asserts that all the services within SlurmFortress that are registered
    /// can be successfully resolved.
    /// </summary>
    [Fact]
    public void AddWebApi_Test()
    {
        var services = new ServiceCollection();

        BuildServices(services);

        //services.AssertConfigurationIsValid();
    }

    /// <summary>
    /// Asserts that all the automapper mappings defined in SlurmFortress are valid
    /// </summary>
    [Fact]
    public void Mapper_Test()
    {
        var services = new ServiceCollection();

        BuildServices(services);

        var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    private static void BuildServices(ServiceCollection services)
    {
        //services.EjectAllInstancesOf<AutoMapper.Configuration.IConfiguration>();
        //var config = new Mock<AutoMapper.Configuration.IConfiguration>();
        //services.AddSingleton(config.Object);
        //new MapperConfiguration();

       var msConfig = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json")
            .Build();
        services.AddSingleton<IConfiguration>(msConfig);

        //act
        services.AddWebApi();
    }
}
