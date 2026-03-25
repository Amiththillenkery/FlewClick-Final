using FlewClick.Application.Features.ConsumerAuth.LoginConsumer;
using FlewClick.Application.Features.ConsumerAuth.RegisterConsumer;
using FlewClick.Application.Features.ConsumerAuth.VerifyLogin;
using FlewClick.Application.Features.ConsumerAuth.VerifyRegistration;
using MediatR;

namespace FlewClick.Api.Endpoints.ConsumerAuth;

public class ConsumerAuthEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (IMediator mediator, RegisterConsumerCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("RegisterConsumer")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/auth/verify-registration", async (IMediator mediator, VerifyRegistrationCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("VerifyRegistration")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/auth/login", async (IMediator mediator, LoginConsumerCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("LoginConsumer")
            .WithTags("Consumer Auth")
            .AllowAnonymous();

        app.MapPost("/api/auth/verify-login", async (IMediator mediator, VerifyLoginCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("VerifyLogin")
            .WithTags("Consumer Auth")
            .AllowAnonymous();
    }
}
