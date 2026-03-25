using FlewClick.Application.Features.PlatformFee.CheckOutstandingFees;
using FlewClick.Application.Features.PlatformFee.GetPaymentHistory;
using FlewClick.Application.Features.PlatformFee.InitiateFeePayment;
using FlewClick.Application.Features.PlatformFee.VerifyFeePayment;
using MediatR;

namespace FlewClick.Api.Endpoints.PlatformFee;

public record InitiateFeePaymentBody(Guid ProfessionalProfileId);

public class PlatformFeeEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/platform-fees/outstanding", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new CheckOutstandingFeesQuery(profileId));
                return Results.Ok(result);
            })
            .WithName("CheckOutstandingFees")
            .WithTags("Platform Fees")
            .RequireAuthorization();

        app.MapPost("/api/platform-fees/{feeId:guid}/initiate", async (IMediator mediator, Guid feeId, InitiateFeePaymentBody body) =>
            {
                var result = await mediator.Send(new InitiateFeePaymentCommand(feeId, body.ProfessionalProfileId));
                return Results.Ok(result);
            })
            .WithName("InitiateFeePayment")
            .WithTags("Platform Fees")
            .RequireAuthorization();

        app.MapPost("/api/platform-fees/verify", async (IMediator mediator, VerifyFeePaymentCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("VerifyFeePayment")
            .WithTags("Platform Fees")
            .AllowAnonymous();

        app.MapGet("/api/platform-fees/history", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new GetPaymentHistoryQuery(profileId));
                return Results.Ok(result);
            })
            .WithName("GetPaymentHistory")
            .WithTags("Platform Fees")
            .RequireAuthorization();
    }
}
