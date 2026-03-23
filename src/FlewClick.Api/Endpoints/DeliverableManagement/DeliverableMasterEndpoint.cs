using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Features.DeliverableManagement.GetDeliverablesByRole;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.DeliverableManagement;

public class DeliverableMasterEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/deliverables/master/{role}", async (IMediator mediator, ProfessionalRole role) =>
                Results.Ok(await mediator.Send(new GetDeliverablesByRoleQuery(role))))
            .WithName("GetDeliverablesByRole")
            .WithTags("Deliverables")
            .Produces<IReadOnlyList<DeliverableMasterDto>>();
    }
}
