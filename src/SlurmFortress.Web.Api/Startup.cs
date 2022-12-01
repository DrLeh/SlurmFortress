using System.Net;
using AutoMapper;
using SlurmFortress.Web.Api.Infrastructure;

namespace SlurmFortress.Web.Api;

public class Startup
{
    private readonly IHostEnvironment _environment;

    static Startup()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _environment = environment;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        //add all standard ASI registrations
        services.AddAsiWebApiStandards(Configuration, _environment);

        //add Microsoft-specific standards
        services.AddMsApi();

        //Add all registrations for this application
        services.AddWebApi();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // Additional services can be injected into this method.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper, ILoggerFactory loggerFactory)
    {
        env.ConfigureLogging(Configuration, loggerFactory);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // seems to cause issues in local and integration tests
            app.UseHttpsRedirection(); //redirects HTTP requests to HTTPS.

            //enable exception handling
        }


        //when hosting to a sub-path, e.g. asiservice.uat-asicentral.com/app, set this path base to
        // that path, and set the <base> tag in index.html
        app.UsePathBase("/slurm");

        app.UseBlazorFrameworkFiles();

        //returns static files and short-circuits further request processing.
        app.UseStaticFiles();
        //app.UseCookiePolicy();

        // Enable Swagger
        app.ConfigureSwagger(Configuration);

        app.UseRouting();

        // Enable CORS, provide policy if need to apply on all controllers, 
        // otherwise enable on individual controller with specific policy
        app.UseCors();

        //app.UseRequestLocalization(); //middleware for localizing into different languages and cultures
        //app.UseResponseCaching();

        app.UseAuthentication(); //attempts to authenticate the user before they're allowed access to secure resources.

        app.UseAuthorization(); // authorizes a user to access secure resources.

        // Enable Response Compression
        app.UseResponseCompression();  //supports gzip and br compression out of the box

        // Insert CorrelationId
        // app.UseRequestResponseLoggingMiddleware(); // This is a local logger middleware for request response logging, useful during development

        app.UseXfo(xfo => xfo.Deny());
        app.UseRedirectValidation(); //Register this earlier if there's middleware that might redirect

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
            //if using reverseProxySettings.json, enable this
            //endpoints.MapReverseProxy();
        });

        //ensure that the automapper config is valid before letting the app run.
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
