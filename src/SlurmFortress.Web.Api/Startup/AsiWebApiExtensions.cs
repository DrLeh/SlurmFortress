using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using SlurmFortress.Web.Api.Infrastructure;

namespace SlurmFortress.Web.Api;

public static class AsiWebApiExtensions
{
    /// <summary>
    /// Standards for all ASI Web applications
    /// </summary>
    public static void AddAsiWebApiStandards(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        //ASI 


        services.AddOptions();
        services.AddResponseCompression();

        //defined in Infrastructure folder
        services.AddSwagger(configuration);

        // Add Controllers
        // Set formatters for API controllers. 
        // Suppress String, XML, and HttpNoContent outputs
        services.AddControllers(options =>
        {
            // requires using Microsoft.AspNetCore.Mvc.Formatters;
            options.OutputFormatters.RemoveType<StringOutputFormatter>();
            options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            options.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();
            //uncomment below line if like to use a generic handler for all exceptions instead of the ASI ExceptionHandler driven by app settings
            //options.Filters.Add<UnhandledExceptionFilterAttribute>(); 
        }).AddControllersAsServices();

        // Add Versioning support for APIs
        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            //o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
        });

    }
}
