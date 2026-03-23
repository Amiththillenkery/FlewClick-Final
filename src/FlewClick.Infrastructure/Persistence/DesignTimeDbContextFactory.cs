using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FlewClick.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FlewClickDbContext>
{
    public FlewClickDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "FlewClick.Api");
        if (!Directory.Exists(basePath))
            basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "src", "FlewClick.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=flewclick;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<FlewClickDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new FlewClickDbContext(optionsBuilder.Options);
    }
}
