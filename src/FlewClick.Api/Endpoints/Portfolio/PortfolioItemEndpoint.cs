using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Features.Portfolio.ReorderPortfolioItems;
using FlewClick.Application.Features.Portfolio.ToggleItemVisibility;
using MediatR;

namespace FlewClick.Api.Endpoints.Portfolio;

public class PortfolioItemEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/portfolio/items/{itemId:guid}/visibility", async (IMediator mediator, Guid itemId) =>
                Results.Ok(await mediator.Send(new ToggleItemVisibilityCommand(itemId))))
            .WithName("TogglePortfolioItemVisibility")
            .WithTags("Portfolio Items")
            .Produces<PortfolioItemDto>()
            .Produces(404);

        app.MapPut("/api/portfolio/{profileId:guid}/reorder", async (IMediator mediator, Guid profileId, ReorderRequest request) =>
            {
                await mediator.Send(new ReorderPortfolioItemsCommand(profileId, request.OrderedItemIds));
                return Results.NoContent();
            })
            .WithName("ReorderPortfolioItems")
            .WithTags("Portfolio Items")
            .Produces(204)
            .Produces(400);
    }
}

public record ReorderRequest(List<Guid> OrderedItemIds);
