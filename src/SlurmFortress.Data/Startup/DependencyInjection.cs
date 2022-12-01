using Microsoft.Extensions.DependencyInjection;
using SlurmFortress.Data.Context;

namespace SlurmFortress.Data;

public static class DependencyInjection
{
    public static void AddData(this IServiceCollection services)
    {
        services.AddDbContext<SlurmFortressDbContext>((sp, options) => options.UseSqlServer(sp
            .GetRequiredService<Core.Configuration.IConfiguration>()!.ConnectionString));

        services.AddScoped<IDataAccess, DataAccess>();
        //inject the NullChangeTrackerManager here when you want to make changes to Lookup tables
        services.AddScoped<IChangeTrackerManager, ChangeTrackerManager>();
    }
}
