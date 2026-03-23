using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.ConfigureEditing;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class ConfigureEditingEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/{profileId:guid}/editing",
                async (IMediator mediator, Guid profileId, ConfigureEditingRequest request) =>
                {
                    var result = await mediator.Send(new ConfigureEditingCommand(
                        profileId, request.SoftwareTools, request.Specializations, request.OutputFormats));
                    return Results.Ok(result);
                })
            .WithName("ConfigureEditing")
            .WithTags("ProfessionalRegistration")
            .Produces<EditingConfigDto>()
            .Produces(400)
            .Produces(404);
    }
}

public record ConfigureEditingRequest(
    List<string> SoftwareTools,
    List<string> Specializations,
    string? OutputFormats);
