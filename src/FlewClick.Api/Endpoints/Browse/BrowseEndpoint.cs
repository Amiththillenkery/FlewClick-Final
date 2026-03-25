using FlewClick.Application.Features.Browse.BrowseProfessionals;
using FlewClick.Application.Features.Browse.GetCategories;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Browse;

public class BrowseEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/browse/categories", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoriesQuery());
                return Results.Ok(result);
            })
            .WithName("GetCategories")
            .WithTags("Browse")
            .AllowAnonymous();

        app.MapGet("/api/browse/professionals", async (
                IMediator mediator,
                ProfessionalRole? role,
                string? location,
                int page = 1,
                int pageSize = 20) =>
            {
                var result = await mediator.Send(new BrowseProfessionalsQuery(role, location, page, pageSize));
                return Results.Ok(result);
            })
            .WithName("BrowseProfessionals")
            .WithTags("Browse")
            .AllowAnonymous();
    }
}
