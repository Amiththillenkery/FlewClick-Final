using FlewClick.Application.Features.ProfileSettings.UpdateOtherProfileOptions;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfileSettings;

public class UpdateOtherProfileOptionsEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/profile-settings/{userId:guid}/professional-options", async (IMediator mediator, Guid userId, UpdateOtherProfileOptionsRequest request) =>
            {
                await mediator.Send(new UpdateOtherProfileOptionsCommand(
                    userId, 
                    request.Bio, 
                    request.Location, 
                    request.YearsOfExperience, 
                    request.HourlyRate, 
                    request.PortfolioUrl));
                return Results.NoContent();
            })
            .WithName("UpdateOtherProfileOptions")
            .WithTags("ProfileSettings")
            .Produces(204)
            .Produces(400)
            .Produces(404);
    }
}

public record UpdateOtherProfileOptionsRequest(
    string? Bio, 
    string? Location, 
    int? YearsOfExperience, 
    decimal? HourlyRate, 
    string? PortfolioUrl);
