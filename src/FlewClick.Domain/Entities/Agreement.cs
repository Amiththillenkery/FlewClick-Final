using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class Agreement : Entity
{
    public Guid BookingRequestId { get; private init; }
    public int Version { get; private init; }
    public string PackageSnapshot { get; private init; } = string.Empty;
    public DateTime EventDate { get; private set; }
    public string? EventLocation { get; private set; }
    public string? EventDescription { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string? Terms { get; private set; }
    public string? Conditions { get; private set; }
    public string? Notes { get; private set; }
    public AgreementStatus Status { get; private set; }

    private Agreement() { }

    public static Agreement Create(
        Guid bookingRequestId, int version, string packageSnapshot,
        DateTime eventDate, decimal totalPrice,
        string? eventLocation = null, string? eventDescription = null,
        string? terms = null, string? conditions = null, string? notes = null)
    {
        if (bookingRequestId == Guid.Empty) throw new DomainException("Booking request ID is required.");
        if (version < 1) throw new DomainException("Version must be at least 1.");
        if (string.IsNullOrWhiteSpace(packageSnapshot)) throw new DomainException("Package snapshot is required.");
        if (totalPrice < 0) throw new DomainException("Total price cannot be negative.");

        return new Agreement
        {
            BookingRequestId = bookingRequestId,
            Version = version,
            PackageSnapshot = packageSnapshot,
            EventDate = eventDate,
            EventLocation = eventLocation?.Trim(),
            EventDescription = eventDescription?.Trim(),
            TotalPrice = totalPrice,
            Terms = terms?.Trim(),
            Conditions = conditions?.Trim(),
            Notes = notes?.Trim(),
            Status = AgreementStatus.Draft
        };
    }

    public void Send()
    {
        if (Status != AgreementStatus.Draft)
            throw new DomainException($"Cannot send agreement in '{Status}' status.");
        Status = AgreementStatus.Sent;
        Touch();
    }

    public void Accept()
    {
        if (Status != AgreementStatus.Sent)
            throw new DomainException($"Cannot accept agreement in '{Status}' status.");
        Status = AgreementStatus.Accepted;
        Touch();
    }

    public void Reject()
    {
        if (Status != AgreementStatus.Sent)
            throw new DomainException($"Cannot reject agreement in '{Status}' status.");
        Status = AgreementStatus.Rejected;
        Touch();
    }

    public void Supersede()
    {
        if (Status == AgreementStatus.Accepted)
            throw new DomainException("Cannot supersede an accepted agreement.");
        Status = AgreementStatus.Superseded;
        Touch();
    }
}
