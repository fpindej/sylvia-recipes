using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Cookies;

namespace Recipes.Infrastructure.Cookies.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCookieServices(this IServiceCollection services)
    {
        services.AddScoped<ICookieService, CookieService>();
        return services;
    }
}
