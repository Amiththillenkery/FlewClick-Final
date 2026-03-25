using System.Text.Json;
using FlewClick.Application.Features.Portfolio.HandleDataDeletion;
using FlewClick.Application.Features.Portfolio.HandleDeauthorize;
using MediatR;

namespace FlewClick.Api.Endpoints.Portfolio;

public class InstagramCallbackEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/portfolio/instagram/deauthorize", async (IMediator mediator, HttpRequest request) =>
            {
                var form = await request.ReadFormAsync();
                var signedRequest = form["signed_request"].ToString();

                var userId = ExtractUserIdFromSignedRequest(signedRequest);
                if (userId is not null)
                {
                    await mediator.Send(new HandleDeauthorizeCommand(userId));
                }

                return Results.Ok();
            })
            .WithName("InstagramDeauthorize")
            .WithTags("Portfolio - Instagram Callbacks")
            .Produces(200);

        app.MapPost("/api/portfolio/instagram/data-deletion", async (IMediator mediator, HttpRequest request) =>
            {
                var form = await request.ReadFormAsync();
                var signedRequest = form["signed_request"].ToString();

                var userId = ExtractUserIdFromSignedRequest(signedRequest);
                if (userId is null)
                    return Results.BadRequest("Could not extract user ID.");

                var result = await mediator.Send(new HandleDataDeletionCommand(userId));
                return Results.Json(new
                {
                    url = result.Url,
                    confirmation_code = result.ConfirmationCode
                });
            })
            .WithName("InstagramDataDeletion")
            .WithTags("Portfolio - Instagram Callbacks")
            .Produces(200)
            .Produces(400);
    }

    private static string? ExtractUserIdFromSignedRequest(string? signedRequest)
    {
        if (string.IsNullOrEmpty(signedRequest)) return null;

        try
        {
            var parts = signedRequest.Split('.');
            if (parts.Length != 2) return null;

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            int remainder = payload.Length % 4;
            if (remainder == 2) payload += "==";
            else if (remainder == 3) payload += "=";

            var jsonBytes = Convert.FromBase64String(payload);
            using var doc = JsonDocument.Parse(jsonBytes);
            return doc.RootElement.TryGetProperty("user_id", out var userIdElement)
                ? userIdElement.ToString()
                : null;
        }
        catch
        {
            return null;
        }
    }
}
