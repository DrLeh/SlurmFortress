using SlurmFortress.Web.Api;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("secureHeaderSettings.json",
                optional: true,
                reloadOnChange: true);
            config.AddJsonFile("ipRateLimitSettings.json",
                optional: true,
                reloadOnChange: true);
            config.AddJsonFile("reverseProxySettings.json",
                optional: true,
                reloadOnChange: true);
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.ConfigureKestrel(x => x.AddServerHeader = false);
        });
