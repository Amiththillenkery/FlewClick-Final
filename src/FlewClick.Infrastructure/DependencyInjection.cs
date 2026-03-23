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

        var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<FlewClickDbContext>(options =>
            options.UseNpgsql(dataSource));

        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
        services.AddScoped<IPhotographyConfigRepository, PhotographyConfigRepository>();
        services.AddScoped<IEditingConfigRepository, EditingConfigRepository>();
        services.AddScoped<IDroneConfigRepository, DroneConfigRepository>();
        services.AddScoped<IRentalEquipmentRepository, RentalEquipmentRepository>();

        services.AddScoped<IDeliverableMasterRepository, DeliverableMasterRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<IPackageDeliverableRepository, PackageDeliverableRepository>();
        services.AddScoped<IPackagePricingRepository, PackagePricingRepository>();
        services.AddScoped<IRentalStoreRepository, RentalStoreRepository>();
        services.AddScoped<IRentalProductRepository, RentalProductRepository>();
        services.AddScoped<IRentalProductImageRepository, RentalProductImageRepository>();
        services.AddScoped<IRentalProductPricingRepository, RentalProductPricingRepository>();

        return services;
    }
}
