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
                string? hub_mode,
                string? hub_verify_token,
                string? hub_challenge) =>
            {
                if (hub_mode is null || hub_verify_token is null || hub_challenge is null)
                    return Results.BadRequest("Missing required parameters.");

                var configuredToken = config["Instagram:VerifyToken"] ?? string.Empty;

                var result = await mediator.Send(new HandleWebhookVerificationQuery(
                    hub_mode, hub_verify_token, hub_challenge, configuredToken));

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
