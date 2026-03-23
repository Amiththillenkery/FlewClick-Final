using FlewClick.Application.Features.ProfessionalRegistration.CompleteRegistration;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class CompleteRegistrationEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/{profileId:guid}/complete",
                async (IMediator mediator, Guid profileId) =>
                {
                    await mediator.Send(new CompleteRegistrationCommand(profileId));
                    return Results.NoContent();
                })
            .WithName("CompleteRegistration")
            .WithTags("ProfessionalRegistration")
            .Produces(204)
            .Produces(400)
            .Produces(404);
    }
}
