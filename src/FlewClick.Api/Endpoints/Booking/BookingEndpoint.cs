using System.Security.Claims;
using FlewClick.Application.Features.Booking.AcceptBookingRequest;
using FlewClick.Application.Features.Booking.CancelBooking;
using FlewClick.Application.Features.Booking.CompleteBooking;
using FlewClick.Application.Features.Booking.CreateBookingRequest;
using FlewClick.Application.Features.Booking.DeclineBookingRequest;
using FlewClick.Application.Features.Booking.GetBookingDetail;
using FlewClick.Application.Features.Booking.GetConsumerBookings;
using FlewClick.Application.Features.Booking.GetIncomingBookings;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Booking;

public record CreateBookingRequestBody(
    Guid ProfessionalProfileId,
    Guid PackageId,
    DateTime EventDate,
    string? EventLocation,
    string? Notes);

public record AcceptBookingRequestBody(Guid ProfessionalProfileId);

public record DeclineBookingRequestBody(Guid ProfessionalProfileId, string Reason);

public record CancelBookingBody(Guid CancelledById, MessageSenderType CancelledByType, string Reason);

public record CompleteBookingBody(Guid ProfessionalProfileId);

public class BookingEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", async (IMediator mediator, ClaimsPrincipal user, CreateBookingRequestBody body) =>
            {
                var consumerId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new CreateBookingRequestCommand(
                    consumerId,
                    body.ProfessionalProfileId,
                    body.PackageId,
                    body.EventDate,
                    body.EventLocation,
                    body.Notes));
                return Results.Created($"/api/bookings/{result.Id}", result);
            })
            .WithName("CreateBookingRequest")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapGet("/api/bookings/my", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var consumerId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new GetConsumerBookingsQuery(consumerId));
                return Results.Ok(result);
            })
            .WithName("GetConsumerBookings")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapGet("/api/bookings/incoming", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new GetIncomingBookingsQuery(profileId));
                return Results.Ok(result);
            })
            .WithName("GetIncomingBookings")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapGet("/api/bookings/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new GetBookingDetailQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetBookingDetail")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{id:guid}/accept", async (IMediator mediator, Guid id, AcceptBookingRequestBody body) =>
            {
                var result = await mediator.Send(new AcceptBookingRequestCommand(id, body.ProfessionalProfileId));
                return Results.Ok(result);
            })
            .WithName("AcceptBookingRequest")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{id:guid}/decline", async (IMediator mediator, Guid id, DeclineBookingRequestBody body) =>
            {
                var result = await mediator.Send(new DeclineBookingRequestCommand(id, body.ProfessionalProfileId, body.Reason));
                return Results.Ok(result);
            })
            .WithName("DeclineBookingRequest")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{id:guid}/cancel", async (IMediator mediator, Guid id, CancelBookingBody body) =>
            {
                var result = await mediator.Send(new CancelBookingCommand(
                    id,
                    body.CancelledById,
                    body.CancelledByType,
                    body.Reason));
                return Results.Ok(result);
            })
            .WithName("CancelBooking")
            .WithTags("Bookings")
            .RequireAuthorization();

        app.MapPost("/api/bookings/{id:guid}/complete", async (IMediator mediator, Guid id, CompleteBookingBody body) =>
            {
                var result = await mediator.Send(new CompleteBookingCommand(id, body.ProfessionalProfileId));
                return Results.Ok(result);
            })
            .WithName("CompleteBooking")
            .WithTags("Bookings")
            .RequireAuthorization();
    }
}
