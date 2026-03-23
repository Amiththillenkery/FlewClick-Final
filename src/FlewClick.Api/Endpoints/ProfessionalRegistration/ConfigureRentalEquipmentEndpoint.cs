using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.ConfigureRentalEquipment;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class ConfigureRentalEquipmentEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/{profileId:guid}/rental-equipment",
                async (IMediator mediator, Guid profileId, AddRentalEquipmentRequest request) =>
                {
                    var result = await mediator.Send(new AddRentalEquipmentCommand(
                        profileId, request.EquipmentName, request.EquipmentType,
                        request.Brand, request.DailyRentalRate, request.ConditionNotes));
                    return Results.Created($"/api/registration/professional/{profileId}/rental-equipment/{result.Id}", result);
                })
            .WithName("AddRentalEquipment")
            .WithTags("ProfessionalRegistration")
            .Produces<RentalEquipmentDto>(201)
            .Produces(400)
            .Produces(404);
    }
}

public record AddRentalEquipmentRequest(
    string EquipmentName,
    string? EquipmentType,
    string? Brand,
    decimal DailyRentalRate,
    string? ConditionNotes);
