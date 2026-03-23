using FlewClick.Application.Features.RentalStoreManagement.Common;
using FlewClick.Application.Features.RentalStoreManagement.ConfigureStore;
using FlewClick.Application.Features.RentalStoreManagement.GetStoreByProfile;
using FlewClick.Application.Features.RentalStoreManagement.UpdateStoreConfig;
using MediatR;

namespace FlewClick.Api.Endpoints.RentalStore;

public class RentalStoreEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rental/store", async (IMediator mediator, ConfigureStoreRequest request) =>
            {
                var result = await mediator.Send(new ConfigureStoreCommand(
                    request.ProfileId, request.StoreName, request.Description, request.Policies));
                return Results.Created($"/api/rental/store/{result.Id}", result);
            })
            .WithName("ConfigureRentalStore")
            .WithTags("RentalStore")
            .Produces<RentalStoreDto>(201)
            .Produces(400);

        app.MapPut("/api/rental/store/{id:guid}", async (IMediator mediator, Guid id, UpdateStoreRequest request) =>
                Results.Ok(await mediator.Send(new UpdateStoreConfigCommand(
                    id, request.StoreName, request.Description, request.Policies))))
            .WithName("UpdateRentalStore")
            .WithTags("RentalStore")
            .Produces<RentalStoreDto>()
            .Produces(404);

        app.MapGet("/api/rental/store/profile/{profileId:guid}", async (IMediator mediator, Guid profileId) =>
                Results.Ok(await mediator.Send(new GetStoreByProfileQuery(profileId))))
            .WithName("GetRentalStoreByProfile")
            .WithTags("RentalStore")
            .Produces<RentalStoreDto>()
            .Produces(404);
    }
}

public record ConfigureStoreRequest(
    Guid ProfileId,
    string StoreName,
    string? Description,
    Dictionary<string, object?>? Policies);

public record UpdateStoreRequest(
    string StoreName,
    string? Description,
    Dictionary<string, object?>? Policies);
