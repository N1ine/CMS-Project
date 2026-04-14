using Client;
using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Замени порт 7001 на тот, на котором крутится твой бекенд!
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5179/") });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

// Наши сервисы
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<ContractsService>();
builder.Services.AddScoped<EmployeeService>();

builder.Services.AddScoped<Client.Services.SearchService>();

await builder.Build().RunAsync();