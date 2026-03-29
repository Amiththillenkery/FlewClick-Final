using System.Security.Claims;
using FlewClick.Application.Features.ProfessionalAuth.GetProfessionalProfile;
using FlewClick.Application.Features.ProfessionalAuth.ProfessionalLogin;
using FlewClick.Application.Features.ProfessionalAuth.RefreshToken;
using FlewClick.Application.Features.ProfessionalAuth.RevokeToken;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalAuth;

public class ProfessionalAuthEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/professional/auth/login", async (IMediator mediator, ProfessionalLoginCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("ProfessionalLogin")
            .WithTags("Professional Auth")
            .AllowAnonymous();

        app.MapPost("/api/professional/auth/refresh", async (IMediator mediator, RefreshTokenCommand command) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("ProfessionalRefreshToken")
            .WithTags("Professional Auth")
            .AllowAnonymous();

        app.MapPost("/api/professional/auth/revoke", async (IMediator mediator, RevokeTokenCommand command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithName("ProfessionalRevokeToken")
            .WithTags("Professional Auth")
            .RequireAuthorization();

        app.MapGet("/api/professional/auth/me", async (IMediator mediator, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new GetProfessionalProfileQuery(userId));
                return Results.Ok(result);
            })
            .WithName("GetProfessionalMe")
            .WithTags("Professional Auth")
            .RequireAuthorization();
    }
}
