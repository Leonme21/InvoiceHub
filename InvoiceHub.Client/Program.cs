using InvoiceHub.Client;
using InvoiceHub.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient base address hacia la API Backend
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7079/") });

// Registrar los servicios de API
builder.Services.AddScoped<ClientApiService>();
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<InvoiceApiService>();
builder.Services.AddScoped<SweetAlertService>();

await builder.Build().RunAsync();
