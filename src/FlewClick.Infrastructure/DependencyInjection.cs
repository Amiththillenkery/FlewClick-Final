using FlewClick.Application.Interfaces;
using FlewClick.Infrastructure.Persistence;
using FlewClick.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlewClick.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=flewclick;Username=postgres;Password=postgres";

        services.AddDbContext<FlewClickDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IAppUserRepository, AppUserRepository>();

        return services;
    }
}
