using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Features.PackageManagement.CreatePackage;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.PackageManagement;

public class CreatePackageEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/packages", async (IMediator mediator, CreatePackageRequest request) =>
            {
                var result = await mediator.Send(new CreatePackageCommand(
                    request.ProfileId, request.Name, request.PackageType,
                    request.Description, request.CoverageType));
                return Results.Created($"/api/packages/{result.Id}", result);
            })
            .WithName("CreatePackage")
            .WithTags("Packages")
            .Produces<PackageDto>(201)
            .Produces(400);
    }
}

public record CreatePackageRequest(
    Guid ProfileId,
    string Name,
    PackageType PackageType,
    string? Description,
    CoverageType? CoverageType);
