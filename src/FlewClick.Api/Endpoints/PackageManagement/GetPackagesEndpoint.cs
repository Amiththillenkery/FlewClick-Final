using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Features.PackageManagement.GetPackageById;
using FlewClick.Application.Features.PackageManagement.GetPackagesByProfile;
using MediatR;

namespace FlewClick.Api.Endpoints.PackageManagement;

public class GetPackagesEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/packages/profile/{profileId:guid}", async (IMediator mediator, Guid profileId) =>
                Results.Ok(await mediator.Send(new GetPackagesByProfileQuery(profileId))))
            .WithName("GetPackagesByProfile")
            .WithTags("Packages")
            .Produces<IReadOnlyList<PackageDto>>();

        app.MapGet("/api/packages/{id:guid}", async (IMediator mediator, Guid id) =>
                Results.Ok(await mediator.Send(new GetPackageByIdQuery(id))))
            .WithName("GetPackageById")
            .WithTags("Packages")
            .Produces<PackageDto>()
            .Produces(404);
    }
}
