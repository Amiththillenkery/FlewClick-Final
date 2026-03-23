using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class DeliverableMaster : Entity
{
    public ProfessionalRole RoleType { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DeliverableCategory Category { get; private set; }
    public Dictionary<string, object?> ConfigurableAttributes { get; private set; } = new();
    public bool IsActive { get; private set; }

    private DeliverableMaster() { }

    public static DeliverableMaster Create(
        ProfessionalRole roleType,
        string name,
        DeliverableCategory category,
        string? description = null,
        Dictionary<string, object?>? configurableAttributes = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Deliverable name is required.");

        if (roleType == ProfessionalRole.DigitalRental)
            throw new DomainException("Deliverables are not applicable for Digital Rental professionals.");

        return new DeliverableMaster
        {
            RoleType = roleType,
            Name = name.Trim(),
            Description = description?.Trim(),
            Category = category,
            ConfigurableAttributes = configurableAttributes ?? new Dictionary<string, object?>(),
            IsActive = true
        };
    }

    public void Update(string name, string? description, DeliverableCategory category,
        Dictionary<string, object?>? configurableAttributes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Deliverable name is required.");

        Name = name.Trim();
        Description = description?.Trim();
        Category = category;
        if (configurableAttributes is not null)
            ConfigurableAttributes = configurableAttributes;
        Touch();
    }

    public void Activate() { IsActive = true; Touch(); }
    public void Deactivate() { IsActive = false; Touch(); }
}
