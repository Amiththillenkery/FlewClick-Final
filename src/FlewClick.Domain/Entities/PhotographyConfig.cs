using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class PhotographyConfig : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public List<string> Styles { get; private set; } = [];
    public string? CameraGear { get; private set; }
    public string? ShootTypes { get; private set; }
    public bool HasStudio { get; private set; }

    private PhotographyConfig() { }

    public static PhotographyConfig Create(
        Guid professionalProfileId,
        List<string> styles,
        string? cameraGear,
        string? shootTypes,
        bool hasStudio)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (styles.Count == 0)
            throw new DomainException("At least one photography style is required.");

        return new PhotographyConfig
        {
            ProfessionalProfileId = professionalProfileId,
            Styles = styles,
            CameraGear = cameraGear?.Trim(),
            ShootTypes = shootTypes?.Trim(),
            HasStudio = hasStudio
        };
    }

    public void Update(
        List<string> styles,
        string? cameraGear,
        string? shootTypes,
        bool hasStudio)
    {
        if (styles.Count == 0)
            throw new DomainException("At least one photography style is required.");

        Styles = styles;
        CameraGear = cameraGear?.Trim();
        ShootTypes = shootTypes?.Trim();
        HasStudio = hasStudio;
        Touch();
    }
}
