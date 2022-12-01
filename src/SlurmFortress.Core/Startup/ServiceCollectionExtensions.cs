using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SlurmFortress.Core;

public static partial class ServiceCollectionExtensions
{
    private static string GetFullPath(string[] paths)
    {
        return string.Join(":", paths.OrEmptyIfNull());
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// using the root path
    /// </summary>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        services.AddOptions();
        services.AddSingleton<IConfigureOptions<TOptions>>(sp => new ConfigureNamedOptions<TOptions>(string.Empty, options =>
        {
            sp.GetService<IConfiguration>()?.Bind(options);
        }));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My:Path"); will bind MyOptions to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, string path)
        where TOptions : class
    {
        services.AddOptions();
        services.AddSingleton<IConfigureOptions<TOptions>>(sp => new ConfigureNamedOptions<TOptions>(string.Empty, options =>
        {
            sp.GetService<IConfiguration>()?.Bind(path, options);
        }));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My","Path"); will bind MyOptions to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, params string[] paths)
        where TOptions : class
    {
        services.AddOptions();
        services.AddSingleton<IConfigureOptions<TOptions>>(sp => new ConfigureNamedOptions<TOptions>(string.Empty, options =>
        {
            sp.GetService<IConfiguration>()?.Bind(GetFullPath(paths), options);
        }));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My:Path"); will bind MyOptions to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, string path, Action<TOptions>? optionsBuilder)
        where TOptions : class
    {
        services.AddOptions();
        services.AddSingleton<IConfigureOptions<TOptions>>(sp => new ConfigureNamedOptions<TOptions>(string.Empty, options =>
        {
            sp.GetService<IConfiguration>()?.Bind(path, options);
            optionsBuilder?.Invoke(options);
        }));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My:Path"); will bind MyOptions to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, IConfiguration config, string path)
        where TOptions : class
    {
        services.AddOptions();
        services.Configure<TOptions>(options => config.Bind(path, options));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My","Path"); will bind MyOptions to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, IConfiguration config, params string[] paths)
        where TOptions : class
    {
        services.AddOptions();
        services.Configure<TOptions>(options => config.Bind(GetFullPath(paths), options));
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="IConfiguration"/>
    /// from the name
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My:Path"); will bind <see cref="TOptions"/> to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="path">Config path e.g. "My:Path"</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, IConfiguration config, string name, Action<TOptions>? optionsBuilder)
        where TOptions : class
    {
        services.AddOptions();
        services.Configure<TOptions>(options =>
        {
            config?.Bind(name, options);
            optionsBuilder?.Invoke(options);
        });
        return services;
    }

    /// <summary>
    /// Configures options class <see cref="TOptions"/> from the registered <see cref="TParentOptions"/>
    /// based on the path. Only allows resolving as IOptions<TChildOptions> or TChildOptions
    /// <para/>
    /// e.g. services.Configure&lt;MyOptions&gt;("My:Path"); will bind <see cref="TOptions"/> to the
    /// config section named "My:Path"
    /// </summary>
    /// <param name="selector">Selector from TParentOptions to find TChildOptions e.g. x => x.Child"</param>
    public static IServiceCollection ConfigureChildOptions<TParentOptions, TChildOptions>(this IServiceCollection services,
        Func<TParentOptions, TChildOptions> selector)
        where TParentOptions : class
        where TChildOptions : class
    {
        services.AddOptions();
        //frustratingly, we are unable to register as ConfigureNamedOptions because that
        // takes an Action, not a Func to create the options
        services.AddSingleton<IOptions<TChildOptions>>(sp =>
        {
            var parent = sp.GetRequiredService<IOptions<TParentOptions>>().Value;
            var child = selector(parent);
            return new OptionsWrapper<TChildOptions>(child);
        });
        services.AddSingleton<TChildOptions>(sp => sp.GetRequiredService<IOptions<TChildOptions>>().Value);
        return services;
    }

    /// <summary>
    /// Gets an options object from the section with the given name
    /// </summary>
    /// <typeparam name="TOptions">Options type to return</typeparam>
    /// <param name="name">Section name. E.g. "MyApp:Setting", name would be "MyApp"</param>
    /// <returns></returns>
    public static TOptions Get<TOptions>(this IConfiguration configuration, string name)
        where TOptions : class, new()
    {
        var opts = new TOptions();
        configuration.GetSection(name).Bind(opts);
        return opts;
    }

    /// <summary>
    /// Removes all instances of type T from the provided service collection.
    /// This is especially useful for unit and integration tests
    /// </summary>
    /// <typeparam name="T">Type to remove</typeparam>
    /// <param name="services">Collection to remove registered type from</param>
    public static void EjectAllInstancesOf<T>(this IServiceCollection services)
    {
        ServiceDescriptor? serviceDescriptor;
        do
        {
            serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);
        }
        while (serviceDescriptor != null);
    }
}
