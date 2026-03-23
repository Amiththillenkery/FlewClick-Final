using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class PackageDeliverable : Entity
{
    public Guid PackageId { get; private init; }
    public Guid DeliverableMasterId { get; private init; }
    public int Quantity { get; private set; }
    public Dictionary<string, object?> Configuration { get; private set; } = new();
    public string? Notes { get; private set; }

    private PackageDeliverable() { }

    public static PackageDeliverable Create(
        Guid packageId,
        Guid deliverableMasterId,
        int quantity,
        Dictionary<string, object?>? configuration = null,
        string? notes = null)
    {
        if (packageId == Guid.Empty)
            throw new DomainException("Package ID is required.");

        if (deliverableMasterId == Guid.Empty)
            throw new DomainException("Deliverable master ID is required.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be at least 1.");

        return new PackageDeliverable
        {
            PackageId = packageId,
            DeliverableMasterId = deliverableMasterId,
            Quantity = quantity,
            Configuration = configuration ?? new Dictionary<string, object?>(),
            Notes = notes?.Trim()
        };
    }

    public void Update(int quantity, Dictionary<string, object?>? configuration, string? notes)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be at least 1.");

        Quantity = quantity;
        if (configuration is not null)
            Configuration = configuration;
        Notes = notes?.Trim();
        Touch();
    }
}
