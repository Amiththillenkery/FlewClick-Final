using FlewClick.Application.Features.Users.UpdateUserProfile;
using MediatR;

namespace FlewClick.Api.Endpoints.Users;

public class UpdateUserProfileEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{id:guid}/profile", async (IMediator mediator, Guid id, UpdateProfileRequest request) =>
            {
                await mediator.Send(new UpdateUserProfileCommand(id, request.FullName, request.Phone));
                return Results.NoContent();
            })
            .WithName("UpdateUserProfile")
            .WithTags("Users")
            .Produces(204)
            .Produces(400)
            .Produces(404);
    }
}

public record UpdateProfileRequest(string FullName, string? Phone);
