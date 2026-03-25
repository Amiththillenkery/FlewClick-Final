using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class Package : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public PackageType PackageType { get; private set; }
    public CoverageType? CoverageType { get; private set; }
    public bool IsActive { get; private set; }

    private Package() { }

    public static Package Create(
        Guid professionalProfileId,
        string name,
        PackageType packageType,
        string? description = null,
        CoverageType? coverageType = null)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Package name is required.");

        return new Package
        {
            ProfessionalProfileId = professionalProfileId,
            Name = name.Trim(),
            Description = description?.Trim(),
            PackageType = packageType,
            CoverageType = coverageType,
            IsActive = true
        };
    }

    public void Update(string name, string? description, PackageType packageType, CoverageType? coverageType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Package name is required.");

        Name = name.Trim();
        Description = description?.Trim();
        PackageType = packageType;
        CoverageType = coverageType;
        Touch();
    }

    public void Activate() { IsActive = true; Touch(); }
    public void Deactivate() { IsActive = false; Touch(); }

    // Navigation Properties
    public virtual ICollection<PackageDeliverable> Deliverables { get; private set; } = new List<PackageDeliverable>();
    public virtual PackagePricing? Pricing { get; private set; }
}
