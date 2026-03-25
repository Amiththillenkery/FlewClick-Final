using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Features.Portfolio.CompleteInstagramAuth;
using FlewClick.Application.Features.Portfolio.DisconnectInstagram;
using FlewClick.Application.Features.Portfolio.GetInstagramStatus;
using FlewClick.Application.Features.Portfolio.InitiateInstagramAuth;
using FlewClick.Application.Features.Portfolio.RefreshInstagramToken;
using MediatR;

namespace FlewClick.Api.Endpoints.Portfolio;

public class InstagramAuthEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/portfolio/instagram/auth-url", async (IMediator mediator, Guid profileId) =>
                Results.Ok(await mediator.Send(new InitiateInstagramAuthCommand(profileId))))
            .WithName("InitiateInstagramAuth")
            .WithTags("Portfolio - Instagram")
            .Produces<InitiateInstagramAuthResponse>()
            .Produces(400);

        app.MapGet("/api/portfolio/instagram/callback", async (IMediator mediator, string code, string state) =>
            {
                if (!Guid.TryParse(state, out var profileId))
                    return Results.BadRequest("Invalid state parameter.");

                var result = await mediator.Send(new CompleteInstagramAuthCommand(code, profileId));
                return Results.Ok(result);
            })
            .WithName("CompleteInstagramAuth")
            .WithTags("Portfolio - Instagram")
            .Produces<InstagramConnectionDto>()
            .Produces(400);

        app.MapGet("/api/portfolio/instagram/{profileId:guid}/status", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new GetInstagramStatusQuery(profileId));
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .WithName("GetInstagramStatus")
            .WithTags("Portfolio - Instagram")
            .Produces<InstagramConnectionDto>()
            .Produces(404);

        app.MapPost("/api/portfolio/instagram/{profileId:guid}/refresh", async (IMediator mediator, Guid profileId) =>
                Results.Ok(await mediator.Send(new RefreshInstagramTokenCommand(profileId))))
            .WithName("RefreshInstagramToken")
            .WithTags("Portfolio - Instagram")
            .Produces<InstagramConnectionDto>()
            .Produces(400);

        app.MapDelete("/api/portfolio/instagram/{profileId:guid}", async (IMediator mediator, Guid profileId, bool clearPortfolio = false) =>
            {
                await mediator.Send(new DisconnectInstagramCommand(profileId, clearPortfolio));
                return Results.NoContent();
            })
            .WithName("DisconnectInstagram")
            .WithTags("Portfolio - Instagram")
            .Produces(204)
            .Produces(404);
    }
}
