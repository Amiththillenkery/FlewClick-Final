using FlewClick.Application.Features.ProfileSettings.UpdateAccountSettings;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfileSettings;

public class UpdateAccountSettingsEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/profile-settings/{userId:guid}/account", async (IMediator mediator, Guid userId, UpdateAccountSettingsRequest request) =>
            {
                await mediator.Send(new UpdateAccountSettingsCommand(userId, request.FullName, request.Phone));
                return Results.NoContent();
            })
            .WithName("UpdateAccountSettings")
            .WithTags("ProfileSettings")
            .Produces(204)
            .Produces(400)
            .Produces(404);
    }
}

public record UpdateAccountSettingsRequest(string FullName, string? Phone);
