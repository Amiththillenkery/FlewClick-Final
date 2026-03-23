namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record EditingConfigDto(
    Guid Id,
    Guid ProfessionalProfileId,
    List<string> SoftwareTools,
    List<string> Specializations,
    string? OutputFormats
);
