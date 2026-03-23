using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.ConfigurePhotography;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class ConfigurePhotographyEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/{profileId:guid}/photography",
                async (IMediator mediator, Guid profileId, ConfigurePhotographyRequest request) =>
                {
                    var result = await mediator.Send(new ConfigurePhotographyCommand(
                        profileId, request.Styles, request.CameraGear, request.ShootTypes, request.HasStudio));
                    return Results.Ok(result);
                })
            .WithName("ConfigurePhotography")
            .WithTags("ProfessionalRegistration")
            .Produces<PhotographyConfigDto>()
            .Produces(400)
            .Produces(404);
    }
}

public record ConfigurePhotographyRequest(
    List<string> Styles,
    string? CameraGear,
    string? ShootTypes,
    bool HasStudio);
