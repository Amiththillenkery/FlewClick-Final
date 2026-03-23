using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Features.PackageManagement.UpdatePackage;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.PackageManagement;

public class UpdatePackageEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/packages/{id:guid}", async (IMediator mediator, Guid id, UpdatePackageRequest request) =>
                Results.Ok(await mediator.Send(new UpdatePackageCommand(
                    id, request.Name, request.PackageType, request.Description, request.CoverageType))))
            .WithName("UpdatePackage")
            .WithTags("Packages")
            .Produces<PackageDto>()
            .Produces(404);
    }
}

public record UpdatePackageRequest(
    string Name,
    PackageType PackageType,
    string? Description,
    CoverageType? CoverageType);
