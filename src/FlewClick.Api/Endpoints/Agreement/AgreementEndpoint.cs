using System.Security.Claims;
using FlewClick.Application.Features.Agreement.AcceptAgreement;
using FlewClick.Application.Features.Agreement.CreateAgreement;
using FlewClick.Application.Features.Agreement.GetAgreement;
using FlewClick.Application.Features.Agreement.GetAgreementHistory;
using FlewClick.Application.Features.Agreement.RequestRevision;
using FlewClick.Application.Features.Agreement.ReviseAgreement;
using MediatR;

namespace FlewClick.Api.Endpoints.Agreement;

public record CreateAgreementRequestBody(
    string PackageSnapshot,
    DateTime EventDate,
    string? EventLocation,
    string? EventDescription,
    decimal TotalPrice,
    string? Terms,
    string? Conditions,
    string? Notes,
    List<CreateAgreementDeliverableItem> Deliverables);

public record ReviseAgreementRequestBody(
    string PackageSnapshot,
    DateTime EventDate,
    string? EventLocation,
    string? EventDescription,
    decimal TotalPrice,
    string? Terms,
    string? Conditions,
    string? Notes,
    List<ReviseAgreementDeliverableItem> Deliverables);

public record RequestRevisionBody(string RevisionNotes);

public class AgreementEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{bookingId:guid}/agreement", async (IMediator mediator, Guid bookingId, CreateAgreementRequestBody body) =>
            {
                var result = await mediator.Send(new CreateAgreementCommand(
                    bookingId,
                    body.PackageSnapshot,
                    body.EventDate,
                    body.EventLocation,
                    body.EventDescription,
                    body.TotalPrice,
                    body.Terms,
                    body.Conditions,
                    body.Notes,
                    body.Deliverables));
                return Results.Created($"/api/bookings/{bookingId}/agreement", result);
            })
            .WithName("CreateAgreement")
            .WithTags("Agreements")
            .RequireAuthorization();

        app.MapGet("/api/bookings/{bookingId:guid}/agreement", async (IMediator mediator, Guid bookingId) =>
            {
                var result = await mediator.Send(new GetAgreementQuery(bookingId));
                return Results.Ok(result);
            })
            .WithName("GetAgreement")
            .WithTags("Agreements")
            .RequireAuthorization();

        app.MapGet("/api/bookings/{bookingId:guid}/agreement/history", async (IMediator mediator, Guid bookingId) =>
            {
                var result = await mediator.Send(new GetAgreementHistoryQuery(bookingId));
                return Results.Ok(result);
            })
            .WithName("GetAgreementHistory")
            .WithTags("Agreements")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{bookingId:guid}/agreement/request-revision", async (IMediator mediator, ClaimsPrincipal user, Guid bookingId, RequestRevisionBody body) =>
            {
                var consumerId = Guid.Parse(user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                await mediator.Send(new RequestRevisionCommand(bookingId, consumerId, body.RevisionNotes));
                return Results.NoContent();
            })
            .WithName("RequestRevision")
            .WithTags("Agreements")
            .RequireAuthorization();

        app.MapPut("/api/bookings/{bookingId:guid}/agreement/revise", async (IMediator mediator, Guid bookingId, ReviseAgreementRequestBody body) =>
            {
                var result = await mediator.Send(new ReviseAgreementCommand(
                    bookingId,
                    body.PackageSnapshot,
                    body.EventDate,
                    body.EventLocation,
                    body.EventDescription,
                    body.TotalPrice,
                    body.Terms,
                    body.Conditions,
                    body.Notes,
                    body.Deliverables));
                return Results.Ok(result);
            })
            .WithName("ReviseAgreement")
            .WithTags("Agreements")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{bookingId:guid}/agreement/accept", async (IMediator mediator, ClaimsPrincipal user, Guid bookingId) =>
            {
                var consumerId = Guid.Parse(user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new AcceptAgreementCommand(bookingId, consumerId));
                return Results.Ok(result);
            })
            .WithName("AcceptAgreement")
            .WithTags("Agreements")
            .RequireAuthorization();
    }
}
