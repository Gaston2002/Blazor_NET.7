using BlazorPelicula.Client;
using BlazorPelicula.Client.Auth;
using BlazorPeliculas.Client;
using BlazorPeliculas.Client.Repositorios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
ConfigureServices(builder.Services);

await builder.Build().RunAsync();

void ConfigureServices(IServiceCollection services)
{
    //cunfiguracion de servicios 
    services.AddSweetAlert2();
    services.AddScoped<IRepositorio, Repositorio>();
    services.AddAuthorizationCore();

    services.AddScoped<ProveedorAuteticacionJWT>();

    services.AddScoped<AuthenticationStateProvider, ProveedorAuteticacionJWT>
        (proveedor => proveedor.GetRequiredService<ProveedorAuteticacionJWT>());

    services.AddScoped<ILoginService, ProveedorAuteticacionJWT>
        (proveedor => proveedor.GetRequiredService<ProveedorAuteticacionJWT>());
}
