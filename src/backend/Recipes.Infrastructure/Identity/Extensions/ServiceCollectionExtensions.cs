using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Identity;
using Recipes.Infrastructure.Identity.Services;

namespace Recipes.Infrastructure.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
