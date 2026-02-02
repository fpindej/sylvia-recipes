using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Features.Recipes;
using Recipes.Infrastructure.Features.Recipes.Services;

namespace Recipes.Infrastructure.Features.Recipes.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds recipe management services to the service collection.
        /// </summary>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddRecipes()
        {
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IEquipmentService, EquipmentService>();

            return services;
        }
    }
}
