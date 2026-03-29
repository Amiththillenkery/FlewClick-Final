using FlewClick.Application.Features.ConsumerAuth.ConsumerRefreshToken;
using FlewClick.Application.Features.ConsumerAuth.ConsumerRevokeToken;
using FlewClick.Application.Features.ConsumerAuth.LoginConsumer;
using FlewClick.Application.Features.ConsumerAuth.RegisterConsumer;
using MediatR;

namespace FlewClick.Api.Endpoints.ConsumerAuth;

public class ConsumerAuthEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/consumer/auth/register", async (IMediator mediator, RegisterConsumerCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Created("/api/consumer/auth/me", result);
            })
            .WithName("RegisterConsumer")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/consumer/auth/login", async (IMediator mediator, LoginConsumerCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("LoginConsumer")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/consumer/auth/refresh", async (IMediator mediator, ConsumerRefreshTokenCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("ConsumerRefreshToken")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/consumer/auth/revoke", async (IMediator mediator, ConsumerRevokeTokenCommand command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithName("ConsumerRevokeToken")
            .WithTags("Consumer Auth")
            .RequireAuthorization();
    }
}
