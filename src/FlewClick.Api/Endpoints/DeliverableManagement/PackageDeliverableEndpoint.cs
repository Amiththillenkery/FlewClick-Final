using FlewClick.Application.Features.DeliverableManagement.AddDeliverableToPackage;
using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Features.DeliverableManagement.GetPackageDeliverables;
using FlewClick.Application.Features.DeliverableManagement.RemoveDeliverableFromPackage;
using FlewClick.Application.Features.DeliverableManagement.UpdatePackageDeliverable;
using MediatR;

namespace FlewClick.Api.Endpoints.DeliverableManagement;

public class PackageDeliverableEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/packages/{packageId:guid}/deliverables", async (IMediator mediator, Guid packageId) =>
                Results.Ok(await mediator.Send(new GetPackageDeliverablesQuery(packageId))))
            .WithName("GetPackageDeliverables")
            .WithTags("Deliverables")
            .Produces<IReadOnlyList<PackageDeliverableDto>>();

        app.MapPost("/api/packages/{packageId:guid}/deliverables",
                async (IMediator mediator, Guid packageId, AddDeliverableRequest request) =>
                {
                    var result = await mediator.Send(new AddDeliverableToPackageCommand(
                        packageId, request.DeliverableMasterId, request.Quantity,
                        request.Configuration, request.Notes));
                    return Results.Created($"/api/packages/{packageId}/deliverables/{result.Id}", result);
                })
            .WithName("AddDeliverableToPackage")
            .WithTags("Deliverables")
            .Produces<PackageDeliverableDto>(201)
            .Produces(400);

        app.MapPut("/api/packages/{packageId:guid}/deliverables/{id:guid}",
                async (IMediator mediator, Guid packageId, Guid id, UpdateDeliverableRequest request) =>
                    Results.Ok(await mediator.Send(new UpdatePackageDeliverableCommand(
                        id, request.Quantity, request.Configuration, request.Notes))))
            .WithName("UpdatePackageDeliverable")
            .WithTags("Deliverables")
            .Produces<PackageDeliverableDto>()
            .Produces(404);

        app.MapDelete("/api/packages/{packageId:guid}/deliverables/{id:guid}",
                async (IMediator mediator, Guid packageId, Guid id) =>
                {
                    await mediator.Send(new RemoveDeliverableFromPackageCommand(id));
                    return Results.NoContent();
                })
            .WithName("RemoveDeliverableFromPackage")
            .WithTags("Deliverables")
            .Produces(204)
            .Produces(404);
    }
}

public record AddDeliverableRequest(
    Guid DeliverableMasterId,
    int Quantity,
    Dictionary<string, object?>? Configuration,
    string? Notes);

public record UpdateDeliverableRequest(
    int Quantity,
    Dictionary<string, object?>? Configuration,
    string? Notes);
