using FlewClick.Application.Features.Users.DeactivateUser;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class DeactivateUserEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/{id:guid}/deactivate", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeactivateUserCommand(id));
                return Results.NoContent();
            })
            .WithName("DeactivateUser")
            .WithTags("Users")
            .Produces(204)
            .Produces(404);
    }
}
