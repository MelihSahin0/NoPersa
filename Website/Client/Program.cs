using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharedLibrary.MappingProfiles;
using System.Text.Json;
using Website.Client;
using Website.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Website.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAutoMapper(typeof(CustomerProfile), typeof(WeekdaysProfile), typeof(MonthlyOverviewProfile), typeof(DailyOverviewProfile));
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton<NavigationContainer>();

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = null
});

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Website.ServerAPI"));

await builder.Build().RunAsync();
