using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class DroneConfig : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string DroneModel { get; private set; } = string.Empty;
    public string? LicenseNumber { get; private set; }
    public bool HasFlightCertification { get; private set; }
    public int? MaxFlightAltitudeMeters { get; private set; }
    public List<string> Capabilities { get; private set; } = [];

    private DroneConfig() { }

    public static DroneConfig Create(
        Guid professionalProfileId,
        string droneModel,
        string? licenseNumber,
        bool hasFlightCertification,
        int? maxFlightAltitudeMeters,
        List<string> capabilities)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(droneModel))
            throw new DomainException("Drone model is required.");

        if (maxFlightAltitudeMeters.HasValue && maxFlightAltitudeMeters <= 0)
            throw new DomainException("Max flight altitude must be positive.");

        return new DroneConfig
        {
            ProfessionalProfileId = professionalProfileId,
            DroneModel = droneModel.Trim(),
            LicenseNumber = licenseNumber?.Trim(),
            HasFlightCertification = hasFlightCertification,
            MaxFlightAltitudeMeters = maxFlightAltitudeMeters,
            Capabilities = capabilities
        };
    }

    public void Update(
        string droneModel,
        string? licenseNumber,
        bool hasFlightCertification,
        int? maxFlightAltitudeMeters,
        List<string> capabilities)
    {
        if (string.IsNullOrWhiteSpace(droneModel))
            throw new DomainException("Drone model is required.");

        DroneModel = droneModel.Trim();
        LicenseNumber = licenseNumber?.Trim();
        HasFlightCertification = hasFlightCertification;
        MaxFlightAltitudeMeters = maxFlightAltitudeMeters;
        Capabilities = capabilities;
        Touch();
    }
}
