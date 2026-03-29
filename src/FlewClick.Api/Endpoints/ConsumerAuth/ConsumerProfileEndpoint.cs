using System.Security.Claims;
using FlewClick.Application.Features.ConsumerAuth.GetConsumerProfile;
using MediatR;

namespace FlewClick.Api.Endpoints.ConsumerAuth;

public class ConsumerProfileEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/consumer/auth/me", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var consumerId = Guid.Parse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new GetConsumerProfileQuery(consumerId));
                return Results.Ok(result);
            })
            .WithName("GetConsumerProfile")
            .WithTags("Consumer Auth")
            .RequireAuthorization();
    }
}
