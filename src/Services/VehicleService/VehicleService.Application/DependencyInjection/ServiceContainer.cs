using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetcodeHub.Packages.Extensions.LocalStorage;
using Vehicle.Application.Extensions;
using Vehicle.Application.Service;
namespace Vehicle.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddAuthorizationCore();
            services.AddNetcodeHubLocalStorageService();
            services.AddScoped<Extensions.LocalStorageService>();
            services.AddScoped<HttpClientService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddTransient<CustomHttpHandler>();
            services.AddCascadingAuthenticationState();
            services.AddHttpClient("WebUIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7158/");
            }).AddHttpMessageHandler<CustomHttpHandler>();
            services.AddScoped<IVehicleService, VehicleService>();  

            return services;
        }
    }
}
