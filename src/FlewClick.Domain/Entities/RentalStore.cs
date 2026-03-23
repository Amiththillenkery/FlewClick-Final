using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RentalStore : Entity
{
    public Guid ProfessionalProfileId { get; private init; }
    public string StoreName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Dictionary<string, object?> Policies { get; private set; } = new();
    public bool IsActive { get; private set; }

    private RentalStore() { }

    public static RentalStore Create(
        Guid professionalProfileId,
        string storeName,
        string? description = null,
        Dictionary<string, object?>? policies = null)
    {
        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required.");

        return new RentalStore
        {
            ProfessionalProfileId = professionalProfileId,
            StoreName = storeName.Trim(),
            Description = description?.Trim(),
            Policies = policies ?? new Dictionary<string, object?>(),
            IsActive = true
        };
    }

    public void Update(string storeName, string? description, Dictionary<string, object?>? policies)
    {
        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required.");

        StoreName = storeName.Trim();
        Description = description?.Trim();
        if (policies is not null)
            Policies = policies;
        Touch();
    }

    public void Activate() { IsActive = true; Touch(); }
    public void Deactivate() { IsActive = false; Touch(); }
}
