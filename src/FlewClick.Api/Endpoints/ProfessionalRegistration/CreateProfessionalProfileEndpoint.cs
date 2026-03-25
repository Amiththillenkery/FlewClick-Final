using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Features.ProfessionalRegistration.CreateProfessionalProfile;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.ProfessionalRegistration;

public class CreateProfessionalProfileEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/registration/professional/profile", async (IMediator mediator, CreateProfileRequest request) =>
            {
                var result = await mediator.Send(new CreateProfessionalProfileCommand(
                    request.FullName, request.Email, request.Phone, request.Password,
                    request.ProfessionalRoles, request.Bio, request.Location,
                    request.YearsOfExperience, request.HourlyRate, request.PortfolioUrl));
                return Results.Created($"/api/registration/professional/{result.Id}/status", result);
            })
            .WithName("CreateProfessionalProfile")
            .WithTags("ProfessionalRegistration")
            .Produces<ProfessionalProfileDto>(201)
            .Produces(400);
    }
}

public record CreateProfileRequest(
    string FullName,
    string Email,
    string? Phone,
    string Password,
    List<ProfessionalRole> ProfessionalRoles,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl);
