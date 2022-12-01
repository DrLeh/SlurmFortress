using ASI.Services.SlurmFortress;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SlurmFortress.Web;
using SlurmFortress.Web.Security;
using SlurmFortress.Core;
using SlurmFortress.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<SlurmFortressWebConfig>(builder.Configuration);
var config = builder.Configuration.Get<SlurmFortressWebConfig>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSlurmFortress();
builder.Services.AddCore();
builder.Services.AddData();

builder.Services
  .AddBlazorise(options =>
  {
      options.Immediate = true;
  })
  .AddBootstrapProviders()
  .AddFontAwesomeIcons();

await builder.Build().RunAsync();
