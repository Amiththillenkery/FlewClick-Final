namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record DroneConfigDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string DroneModel,
    string? LicenseNumber,
    bool HasFlightCertification,
    int? MaxFlightAltitudeMeters,
    List<string> Capabilities
);
