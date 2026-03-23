using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Features.PackageManagement.DeletePackage;
using FlewClick.Application.Features.PackageManagement.TogglePackageStatus;
using MediatR;

namespace FlewClick.Api.Endpoints.PackageManagement;

public class PackageStatusEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/packages/{id:guid}/status", async (IMediator mediator, Guid id, ToggleStatusRequest request) =>
                Results.Ok(await mediator.Send(new TogglePackageStatusCommand(id, request.Activate))))
            .WithName("TogglePackageStatus")
            .WithTags("Packages")
            .Produces<PackageDto>()
            .Produces(404);

        app.MapDelete("/api/packages/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeletePackageCommand(id));
                return Results.NoContent();
            })
            .WithName("DeletePackage")
            .WithTags("Packages")
            .Produces(204)
            .Produces(404);
    }
}

public record ToggleStatusRequest(bool Activate);
