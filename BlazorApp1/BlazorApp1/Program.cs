using AdminPanel; // Change this to your actual namespace
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Add MudBlazor
builder.Services.AddMudServices();

// 2. Add Supabase (Connects to your REAL Mobile App DB)
var supabaseUrl = "https://your-project-id.supabase.co"; // REPLACE THIS
var supabaseKey = "your-public-anon-key";              // REPLACE THIS

var options = new Supabase.SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
};
var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, options);
await supabaseClient.InitializeAsync();

builder.Services.AddSingleton(supabaseClient);

await builder.Build().RunAsync();