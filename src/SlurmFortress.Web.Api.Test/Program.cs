using Microsoft.Extensions.Configuration;

namespace SlurmFortress.Web.Api.Test;

static class Program
{
    public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
    {
        return new ConfigurationBuilder()
            .SetBasePath(outputPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
