using Goke.Core.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestWeb.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

// Add other services
builder.Services.AddTransient<IFormFactor, FormFactorService>();


await builder.Build().RunAsync();
