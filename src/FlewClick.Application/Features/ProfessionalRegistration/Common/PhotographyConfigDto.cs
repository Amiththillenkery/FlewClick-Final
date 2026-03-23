namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record PhotographyConfigDto(
    Guid Id,
    Guid ProfessionalProfileId,
    List<string> Styles,
    string? CameraGear,
    string? ShootTypes,
    bool HasStudio
);
