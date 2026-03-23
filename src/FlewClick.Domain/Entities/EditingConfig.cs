using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class EditingConfig : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public List<string> SoftwareTools { get; private set; } = [];
    public List<string> Specializations { get; private set; } = [];
    public string? OutputFormats { get; private set; }

    private EditingConfig() { }

    public static EditingConfig Create(
        Guid professionalProfileId,
        List<string> softwareTools,
        List<string> specializations,
        string? outputFormats)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (softwareTools.Count == 0)
            throw new DomainException("At least one software tool is required.");

        return new EditingConfig
        {
            ProfessionalProfileId = professionalProfileId,
            SoftwareTools = softwareTools,
            Specializations = specializations,
            OutputFormats = outputFormats?.Trim()
        };
    }

    public void Update(
        List<string> softwareTools,
        List<string> specializations,
        string? outputFormats)
    {
        if (softwareTools.Count == 0)
            throw new DomainException("At least one software tool is required.");

        SoftwareTools = softwareTools;
        Specializations = specializations;
        OutputFormats = outputFormats?.Trim();
        Touch();
    }
}
