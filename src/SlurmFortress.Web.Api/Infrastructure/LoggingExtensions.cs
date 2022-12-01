
namespace SlurmFortress.Web.Api.Infrastructure;

/// <summary>
/// Register's logging using ASI's standards
/// </summary>
public static class LoggingExtensions
{

    /// <summary>
    /// Configure Logging
    /// </summary>
    public static void ConfigureLogging(this IWebHostEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddLog4Net();

        var logger = loggerFactory.CreateLogger("Startup");
        var (isDev, isStaging, isProduction) = (env.IsDevelopment(), env.IsStaging(), env.IsProduction());
        logger.LogInformation($"Starting Mars.Web.Api, IsDevelopment={isDev} IsStaging={isStaging} IsProd={isProduction}");
    }
}
