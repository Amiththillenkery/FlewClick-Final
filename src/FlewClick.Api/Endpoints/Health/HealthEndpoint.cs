using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Api.Endpoints.Health;

public class HealthEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/healthz", async (FlewClickDbContext dbContext) =>
        {
            try
            {
                // Simple DB check
                var canConnect = await dbContext.Database.CanConnectAsync();
                
                if (!canConnect)
                    return Results.Problem("Database connection failed", statusCode: 503);

                return Results.Ok(new
                {
                    Status = "Healthy",
                    Database = "Connected",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 503);
            }
        })
        .WithName("HealthCheck")
        .WithTags("Infrastructure")
        .AllowAnonymous();
    }
}
