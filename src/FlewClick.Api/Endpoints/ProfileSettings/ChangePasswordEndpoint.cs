using FlewClick.Application.Features.ProfileSettings.ChangePassword;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfileSettings;

public class ChangePasswordEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/profile-settings/{userId:guid}/password", async (IMediator mediator, Guid userId, ChangePasswordRequest request) =>
            {
                await mediator.Send(new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword));
                return Results.NoContent();
            })
            .WithName("ChangePassword")
            .WithTags("ProfileSettings")
            .Produces(204)
            .Produces(400)
            .Produces(404);
    }
}

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
