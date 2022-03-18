using CreITBuy.Core.Contracts;
using CreITBuy.Core.Services;
using CreITBuy.Infrastructure.Data.Repositoryes;
using CreITBuy.Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CreITBuy.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
