using Microsoft.Extensions.DependencyInjection;
using SlurmFortress.Core.Configuration;
using SlurmFortress.Core.Context;
using SlurmFortress.Core.Data;
using SlurmFortress.Core.MappingProfiles;
using SlurmFortress.Core.Slurms;

namespace SlurmFortress.Core;

public static class DependencyInjection
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SlurmProfile).Assembly);

        services.AddSingleton<IConfiguration, FileConfiguration>();

        services.AddScoped<ISlurmLoader, SlurmLoader>();
        services.AddScoped<ISlurmService, SlurmService>();

        services.AddScoped<IAuditFieldFixer, AuditFieldFixer>();

        services.AddSingleton<IUserInformation, TestUserInformation>();
    }
}
