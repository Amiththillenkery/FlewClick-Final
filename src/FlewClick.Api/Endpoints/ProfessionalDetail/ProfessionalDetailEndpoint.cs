using FlewClick.Application.Features.ProfessionalDetail.GetProfessionalDetail;
using FlewClick.Application.Features.ProfessionalDetail.GetProfessionalPackages;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalDetail;

public class ProfessionalDetailEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/professionals/{profileId:guid}/detail", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new GetProfessionalDetailQuery(profileId));
                return Results.Ok(result);
            })
            .WithName("GetProfessionalDetail")
            .WithTags("Professional Detail")
            .AllowAnonymous();

        app.MapGet("/api/professionals/{profileId:guid}/packages", async (IMediator mediator, Guid profileId) =>
            {
                var result = await mediator.Send(new GetProfessionalPackagesQuery(profileId));
                return Results.Ok(result);
            })
            .WithName("GetProfessionalPackages")
            .WithTags("Professional Detail")
            .AllowAnonymous();
    }
}
