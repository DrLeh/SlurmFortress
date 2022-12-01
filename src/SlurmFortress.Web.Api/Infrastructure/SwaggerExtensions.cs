using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SlurmFortress.Web.Api.Infrastructure;

/// <summary>
/// Swagger setup methods
/// </summary>
public static partial class SwaggerExtensions
{
    private const string AppName = "SlurmFortress";

    /// <summary>
    /// Configure the Swagger endpoints for this app
    /// </summary>
    public static void ConfigureSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        //Enable Swagger API documentation support and SwaggerUI
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"v1/swagger.json", $"{AppName} v1");
            // uncomment below lines if application uses Azure Ad auth
            //c.OAuthClientId(configuration["AzureAd:ClientId"]);
            //c.OAuthUsePkce();
        });
    }

    /// <summary>
    /// Register's swagger using ASI's standards
    /// </summary>
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        // uncomment below lines if application uses Azure Ad auth
        //var azureAdSettings = new AzureAdSettings();
        //configuration.Bind("AzureAd", azureAdSettings);

        services.AddSwaggerGen(options =>
        {
            // Tell swagger to configure the name of the app to show as the title
            //  and tell it to use the specified markdown file as the description. 
            //options.ConfigureSwaggerDocFromResouce($"ASI {AppName}", "SlurmFortress.Web.Api.Infrastructure.SwaggerDescription.md");
            //options.ConfigureSwaggerDoc($"ASI {AppName}", "SlurmFortress Description");

            //// one line solution for adding Azure AD authentication support in swagger
            //// uncomment below line if application uses Azure Ad auth
            ////options.AddAsiAzureAdSecurity(azureAdSettings);

            //// one line solution for adding JWT authentication support in swagger
            //// comment or remove below line if application uses Azure Ad auth
            //options.AddAsiJwtSecurity();

            //// tell swagger to look at the C# XML documentation from the this assembly
            //options.AddXmlDocumentationFromAssembly<Startup>();

            //// tell swagger to add the C# XML documentation from the assembly containing contracts
            //options.AddXmlDocumentationFromAssembly<SlurmView>();
        });
    }
}
