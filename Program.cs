using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using key_to_mallorca_wasm;
using key_to_mallorca_wasm.Controllers;
using key_to_mallorca_wasm.Models;
using key_to_mallorca_wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IQuestionDataService, OnlineQuestionDataService>();
builder.Services.AddScoped<SpeciesService>();
builder.Services.AddScoped<ISpeciesDetails, SpeciesController>();
builder.Services.AddScoped<IKey, KeyController>();


await builder.Build().RunAsync();
