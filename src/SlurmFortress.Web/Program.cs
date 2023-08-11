using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SlurmFortress.Web;
using SlurmFortress.Web.Game;
using SlurmFortress.Web.Game.Time;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<SlurmFortressWebConfig>(builder.Configuration);
var config = builder.Configuration.Get<SlurmFortressWebConfig>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSlurmFortressApi();
builder.Services.AddCore();
builder.Services.AddData();

builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly));
builder.Services.AddSingleton(GameTimer.Instance);
builder.Services.AddScoped<GameTicker>();

builder.Services
  .AddBlazorise(options =>
  {
      options.Immediate = true;
  })
  .AddBootstrapProviders()
  .AddFontAwesomeIcons();

await builder.Build().RunAsync();
