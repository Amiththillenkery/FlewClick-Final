using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public static class RegistrationMapper
{
    public static ProfessionalProfileDto ToDto(ProfessionalProfile p) =>
        new(p.Id, p.AppUserId, p.Bio, p.Location, p.YearsOfExperience,
            p.HourlyRate, p.PortfolioUrl, p.IsRegistrationComplete,
            p.CreatedAtUtc, p.UpdatedAtUtc);

    public static PhotographyConfigDto ToDto(PhotographyConfig c) =>
        new(c.Id, c.ProfessionalProfileId, c.Styles, c.CameraGear, c.ShootTypes, c.HasStudio);

    public static EditingConfigDto ToDto(EditingConfig c) =>
        new(c.Id, c.ProfessionalProfileId, c.SoftwareTools, c.Specializations, c.OutputFormats);

    public static DroneConfigDto ToDto(DroneConfig c) =>
        new(c.Id, c.ProfessionalProfileId, c.DroneModel, c.LicenseNumber,
            c.HasFlightCertification, c.MaxFlightAltitudeMeters, c.Capabilities);

    public static RentalEquipmentDto ToDto(RentalEquipment e) =>
        new(e.Id, e.ProfessionalProfileId, e.EquipmentName, e.EquipmentType,
            e.Brand, e.DailyRentalRate, e.IsAvailable, e.ConditionNotes);
}
