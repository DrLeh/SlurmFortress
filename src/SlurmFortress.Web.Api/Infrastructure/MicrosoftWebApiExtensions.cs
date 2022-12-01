using Newtonsoft.Json;

namespace SlurmFortress.Web.Api.Infrastructure;

public static class MicrosoftWebApiExtensions
{
    /// <summary>
    /// Microsoft-specific registrations
    /// </summary>
    public static void AddMsApi(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllers();

        //make controllers lowercase
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddMvc()
            .AddNewtonsoftJson(o =>
            {
                //no resolver = don't change anything = keep it PascalCase
                o.SerializerSettings.ContractResolver = null;
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            })
            .AddJsonOptions(options =>
            {
                //this seems to be necessary for swagger to use correct naming policy
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            })
            ;
    }
}
