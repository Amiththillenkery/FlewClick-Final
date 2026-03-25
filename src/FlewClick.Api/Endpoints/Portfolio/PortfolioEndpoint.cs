using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Features.Portfolio.GetPortfolio;
using FlewClick.Application.Features.Portfolio.SyncPortfolio;
using MediatR;

namespace FlewClick.Api.Endpoints.Portfolio;

public class PortfolioEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/portfolio/{profileId:guid}/sync", async (IMediator mediator, Guid profileId) =>
                Results.Ok(await mediator.Send(new SyncPortfolioCommand(profileId))))
            .WithName("SyncPortfolio")
            .WithTags("Portfolio")
            .Produces<SyncPortfolioResponse>()
            .Produces(400);

        app.MapGet("/api/portfolio/{profileId:guid}", async (IMediator mediator, Guid profileId, bool visibleOnly = true) =>
                Results.Ok(await mediator.Send(new GetPortfolioQuery(profileId, visibleOnly))))
            .WithName("GetPortfolio")
            .WithTags("Portfolio")
            .Produces<List<PortfolioItemDto>>();
    }
}
