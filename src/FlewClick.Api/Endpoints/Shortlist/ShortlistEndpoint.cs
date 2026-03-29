using System.Security.Claims;
using FlewClick.Application.Features.Shortlist.GetSavedProfessionals;
using FlewClick.Application.Features.Shortlist.RemoveSavedProfessional;
using FlewClick.Application.Features.Shortlist.SaveProfessional;
using MediatR;

namespace FlewClick.Api.Endpoints.Shortlist;

public class ShortlistEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/shortlist/{profileId:guid}", async (IMediator mediator, ClaimsPrincipal user, Guid profileId) =>
            {
                var consumerId = Guid.Parse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                await mediator.Send(new SaveProfessionalCommand(consumerId, profileId));
                return Results.Created($"/api/shortlist/{profileId}", null);
            })
            .WithName("SaveProfessional")
            .WithTags("Shortlist")
            .RequireAuthorization();

        app.MapDelete("/api/shortlist/{profileId:guid}", async (IMediator mediator, ClaimsPrincipal user, Guid profileId) =>
            {
                var consumerId = Guid.Parse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                await mediator.Send(new RemoveSavedProfessionalCommand(consumerId, profileId));
                return Results.NoContent();
            })
            .WithName("RemoveSavedProfessional")
            .WithTags("Shortlist")
            .RequireAuthorization();

        app.MapGet("/api/shortlist", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var consumerId = Guid.Parse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new GetSavedProfessionalsQuery(consumerId));
                return Results.Ok(result);
            })
            .WithName("GetSavedProfessionals")
            .WithTags("Shortlist")
            .RequireAuthorization();
    }
}
