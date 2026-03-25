namespace FlewClick.Domain.Enums;

public enum BookingStatus
{
    Requested = 0,
    PendingQuotation = 1,
    QuotationSent = 2,
    RevisionRequested = 3,
    Accepted = 4,
    Active = 5,
    Completed = 6,
    Declined = 7,
    Cancelled = 8
}
