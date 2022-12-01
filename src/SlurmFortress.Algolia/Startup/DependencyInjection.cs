using ASI.Services.Access;
using ASI.Services.Access.Search;
using ASI.Services.Algolia.Client;
using ASI.Services.Search.Models;
using Microsoft.Extensions.DependencyInjection;
using SlurmFortress.Algolia.Indexing;
using SlurmFortress.Algolia.Indexing.Seeding;
using SlurmFortress.Algolia.Querying;
using SlurmFortress.Core.Models;
using SlurmFortress.Core.Search.Models;
using SlurmFortress.Core.Search.Providers;

namespace SlurmFortress.Algolia;

public static class DependencyInjection
{
    public static void AddAlgolia(this IServiceCollection services)
    {
        services.AddAsiAlgolia();
        services.AddAsiAlgoliaIndexing();
        services.AddAsiAlgoliaSecurity();

        services.AddSingleton<ISearchIndexNameProvider, AlgoliaIndexNameProvider>();

        RegisterSearchProviders(services);
        AddIndexOperationStrategies(services);
        RegisterIndexCreation(services);
        RegisterSeeding(services);
        RegisterSearchSecurity(services);
    }

    private static void RegisterSearchProviders(IServiceCollection services)
    {
        services.AddScoped<ISlurmSearchProvider, SlurmSearchProvider>();
    }

    private static void AddIndexOperationStrategies(IServiceCollection services)
    {
        services.AddSingleton<IIndexOperationStrategy, DefaultAutomapperIndexOperationStrategy<Slurm, SearchSlurm>>();
    }

    private static void RegisterIndexCreation(IServiceCollection services)
    {
        services.AddSingleton<IIndexDefinition<SearchSlurm>, SlurmIndexDefinition>();
        services.AddSingleton<IIndexCreationStrategy, DefaultIndexCreationStrategy<SearchSlurm>>();
        services.AddSingleton<IIndexOperationStrategy, DefaultAutomapperIndexOperationStrategy<Slurm, SearchSlurm>>();
    }

    private static void RegisterSeeding(IServiceCollection services)
    {
        services.AddScoped<AlgoliaSeedService>();
        services.AddScoped<ISeeder, SlurmSeeder>();

        services.AddScoped<SeederProvider>();
    }

    private static void RegisterSearchSecurity(IServiceCollection services)
    {
        //for generic entity that needs to be filtered by tenantid, use this
        RegisterTenantFilter<SearchSlurm>(services);
        //if object is ASI.Services.Access.IShareable, use this
        //RegisterShareableFilter<SearchSlurm>(services);
    }

    /// <summary>
    /// Registeres search security filters for Shareability
    /// This will enable Checking Access string during search based on current user's permissions)
    /// </summary>
    private static void RegisterTenantFilter<T>(IServiceCollection services)
        where T : ISearchModel
    {
        services.AddScoped<ISearchSecurityFilter, GenericTenantReadableSearchFilter<T>>();
        services.AddScoped<ISearchSecurityFilter, GenericTenantEditableSearchFilter<T>>();
    }

    /// <summary>
    /// Registeres search security filters for Shareability
    /// This will enable Checking Access string during search based on current user's permissions)
    /// </summary>
    private static void RegisterShareableFilter<T>(IServiceCollection services)
        where T : IShareable
    {
        services.AddScoped<ISearchSecurityFilter, ShareableReadableSearchModelFilter<T>>();
        services.AddScoped<ISearchSecurityFilter, ShareableSearchEditableModelFilter<T>>();
    }
}
