using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class BookingRequest : Entity
{
    public Guid ConsumerId { get; private init; }
    public Guid ProfessionalProfileId { get; private init; }
    public Guid PackageId { get; private init; }
    public DateTime EventDate { get; private set; }
    public string? EventLocation { get; private set; }
    public string? Notes { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? DeclineReason { get; private set; }
    public string? CancellationReason { get; private set; }
    public MessageSenderType? CancelledBy { get; private set; }

    private BookingRequest() { }

    private static readonly BookingStatus[] TerminalStatuses =
        [BookingStatus.Completed, BookingStatus.Declined, BookingStatus.Cancelled];

    public static BookingRequest Create(
        Guid consumerId, Guid professionalProfileId, Guid packageId,
        DateTime eventDate, string? eventLocation = null, string? notes = null)
    {
        if (consumerId == Guid.Empty) throw new DomainException("Consumer ID is required.");
        if (professionalProfileId == Guid.Empty) throw new DomainException("Professional profile ID is required.");
        if (packageId == Guid.Empty) throw new DomainException("Package ID is required.");
        if (eventDate <= DateTime.UtcNow) throw new DomainException("Event date must be in the future.");

        return new BookingRequest
        {
            ConsumerId = consumerId,
            ProfessionalProfileId = professionalProfileId,
            PackageId = packageId,
            EventDate = eventDate,
            EventLocation = eventLocation?.Trim(),
            Notes = notes?.Trim(),
            Status = BookingStatus.Requested
        };
    }

    public void AcceptRequest()
    {
        GuardTransition(BookingStatus.Requested, "accept");
        Status = BookingStatus.PendingQuotation;
        Touch();
    }

    public void Decline(string reason)
    {
        GuardTransition(BookingStatus.Requested, "decline");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Decline reason is required.");
        Status = BookingStatus.Declined;
        DeclineReason = reason.Trim();
        Touch();
    }

    public void MarkQuotationSent()
    {
        if (Status != BookingStatus.PendingQuotation && Status != BookingStatus.RevisionRequested)
            throw new DomainException($"Cannot send quotation when status is '{Status}'. Expected 'PendingQuotation' or 'RevisionRequested'.");
        Status = BookingStatus.QuotationSent;
        Touch();
    }

    public void RequestRevision()
    {
        GuardTransition(BookingStatus.QuotationSent, "request revision");
        Status = BookingStatus.RevisionRequested;
        Touch();
    }

    public void AcceptAgreement()
    {
        GuardTransition(BookingStatus.QuotationSent, "accept agreement");
        Status = BookingStatus.Accepted;
        Touch();
    }

    public void Activate()
    {
        GuardTransition(BookingStatus.Accepted, "activate");
        Status = BookingStatus.Active;
        Touch();
    }

    public void Complete()
    {
        GuardTransition(BookingStatus.Active, "complete");
        Status = BookingStatus.Completed;
        Touch();
    }

    public void Cancel(string reason, MessageSenderType cancelledBy)
    {
        if (TerminalStatuses.Contains(Status))
            throw new DomainException($"Cannot cancel a booking that is already '{Status}'.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Cancellation reason is required.");

        Status = BookingStatus.Cancelled;
        CancellationReason = reason.Trim();
        CancelledBy = cancelledBy;
        Touch();
    }

    private void GuardTransition(BookingStatus expected, string action)
    {
        if (Status != expected)
            throw new DomainException($"Cannot {action} when status is '{Status}'. Expected '{expected}'.");
    }
}
