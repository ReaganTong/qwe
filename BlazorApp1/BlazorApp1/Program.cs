using BlazorApp1;
using Blazored.LocalStorage;
using CampusSafety.Admin;
using CampusSafety.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add services
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// Add HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Add custom services
builder.Services.AddScoped<SupabaseService>();
builder.Services.AddScoped<IncidentService>();
builder.Services.AddScoped<ChartService>();
builder.Services.AddScoped<UserService>();

// Configure Supabase
builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var url = configuration["Supabase:Url"];
    var key = configuration["Supabase:Key"];

    var options = new Supabase.SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true,
        SessionHandler = new SupabaseSessionHandler()
    };

    return new Supabase.Client(url, key, options);
});

await builder.Build().RunAsync();