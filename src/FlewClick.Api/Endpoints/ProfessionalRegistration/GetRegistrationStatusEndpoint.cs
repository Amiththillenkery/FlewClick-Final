using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.GetRegistrationStatus;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class GetRegistrationStatusEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/registration/professional/{profileId:guid}/status",
                async (IMediator mediator, Guid profileId) =>
                {
                    var result = await mediator.Send(new GetRegistrationStatusQuery(profileId));
                    return Results.Ok(result);
                })
            .WithName("GetRegistrationStatus")
            .WithTags("ProfessionalRegistration")
            .Produces<RegistrationStatusDto>()
            .Produces(404);
    }
}
