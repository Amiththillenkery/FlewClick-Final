using FlewClick.Application.Features.Portfolio.HandleWebhookVerification;
using MediatR;

namespace FlewClick.Api.Endpoints.Portfolio;

public class InstagramWebhookEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/portfolio/instagram/webhook", async (
                IMediator mediator,
                IConfiguration config,
                [Microsoft.AspNetCore.Mvc.FromQuery(Name = "hub.mode")] string? hubMode,
                [Microsoft.AspNetCore.Mvc.FromQuery(Name = "hub.verify_token")] string? hubVerifyToken,
                [Microsoft.AspNetCore.Mvc.FromQuery(Name = "hub.challenge")] string? hubChallenge) =>
            {
                if (hubMode is null || hubVerifyToken is null || hubChallenge is null)
                    return Results.BadRequest("Missing required parameters.");

                var configuredToken = config["Instagram:VerifyToken"] ?? string.Empty;

                var result = await mediator.Send(new HandleWebhookVerificationQuery(
                    hubMode, hubVerifyToken, hubChallenge, configuredToken));

                return result is not null
                    ? Results.Text(result)
                    : Results.StatusCode(403);
            })
            .WithName("VerifyInstagramWebhook")
            .WithTags("Portfolio - Instagram Webhook")
            .Produces<string>(200)
            .Produces(403);
    }
}
