using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Persistence;
using Recipes.Infrastructure.Features.Authentication.Extensions;
using Recipes.Infrastructure.Persistence.Interceptors;

namespace Recipes.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            services.ConfigureDbContext(configuration);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));

            return services;
        }

        public IServiceCollection AddIdentityServices(IConfiguration configuration)
        {
            services.AddIdentity<RecipesDbContext>(configuration);

            return services;
        }

        private IServiceCollection ConfigureDbContext(IConfiguration configuration)
        {
            services.AddScoped<AuditingInterceptor>();
            services.AddScoped<UserCacheInvalidationInterceptor>();
            services.AddDbContext<RecipesDbContext>((sp, opt) =>
            {
                var connectionString = configuration.GetConnectionString("Database");
                opt.UseNpgsql(connectionString);
                opt.AddInterceptors(
                    sp.GetRequiredService<AuditingInterceptor>(),
                    sp.GetRequiredService<UserCacheInvalidationInterceptor>());
            });
            return services;
        }
    }
}
