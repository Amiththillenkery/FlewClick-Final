using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class AgreementDeliverable : Entity
{
    public Guid AgreementId { get; private init; }
    public string DeliverableName { get; private init; } = string.Empty;
    public int Quantity { get; private init; }
    public Dictionary<string, object?> Configuration { get; private init; } = new();
    public string? Notes { get; private init; }

    private AgreementDeliverable() { }

    public static AgreementDeliverable Create(
        Guid agreementId, string deliverableName, int quantity,
        Dictionary<string, object?>? configuration = null, string? notes = null)
    {
        if (agreementId == Guid.Empty) throw new DomainException("Agreement ID is required.");
        if (string.IsNullOrWhiteSpace(deliverableName)) throw new DomainException("Deliverable name is required.");
        if (quantity <= 0) throw new DomainException("Quantity must be at least 1.");

        return new AgreementDeliverable
        {
            AgreementId = agreementId,
            DeliverableName = deliverableName.Trim(),
            Quantity = quantity,
            Configuration = configuration ?? new Dictionary<string, object?>(),
            Notes = notes?.Trim()
        };
    }
}
