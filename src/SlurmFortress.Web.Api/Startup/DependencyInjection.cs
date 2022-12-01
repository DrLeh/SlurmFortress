using SlurmFortress.Core.Context;
using SlurmFortress.Core;
using SlurmFortress.Data;

namespace SlurmFortress.Web.Api;

public static class DependencyInjection
{
    /// <summary>
    /// SlurmFortress registrations. This is where to register all objects needed by your code
    /// </summary>
    public static void AddWebApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddLogging();

        RegisterUser(services);

        services.AddCore();
        services.AddData();
    }

    /// <summary>
    /// Register all things pertaining to the current user
    /// </summary>
    private static void RegisterUser(IServiceCollection services)
    {
        //register a service which has the current user information
        //services.AddScoped<IUserInformation, WebUserInformation>();

    }
}
