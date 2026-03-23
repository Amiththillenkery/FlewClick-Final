using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.ConfigureDrone;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class ConfigureDroneEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/{profileId:guid}/drone",
                async (IMediator mediator, Guid profileId, ConfigureDroneRequest request) =>
                {
                    var result = await mediator.Send(new ConfigureDroneCommand(
                        profileId, request.DroneModel, request.LicenseNumber,
                        request.HasFlightCertification, request.MaxFlightAltitudeMeters, request.Capabilities));
                    return Results.Ok(result);
                })
            .WithName("ConfigureDrone")
            .WithTags("ProfessionalRegistration")
            .Produces<DroneConfigDto>()
            .Produces(400)
            .Produces(404);
    }
}

public record ConfigureDroneRequest(
    string DroneModel,
    string? LicenseNumber,
    bool HasFlightCertification,
    int? MaxFlightAltitudeMeters,
    List<string> Capabilities);
