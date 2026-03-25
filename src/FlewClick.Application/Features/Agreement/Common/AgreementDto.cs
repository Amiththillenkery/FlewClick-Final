using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Agreement.Common;

public record AgreementDto(
    Guid Id,
    Guid BookingRequestId,
    int Version,
    string PackageSnapshot,
    DateTime EventDate,
    string? EventLocation,
    string? EventDescription,
    decimal TotalPrice,
    string? Terms,
    string? Conditions,
    string? Notes,
    AgreementStatus Status,
    List<AgreementDeliverableDto> Deliverables,
    DateTime CreatedAtUtc
);

public record AgreementDeliverableDto(
    Guid Id,
    string DeliverableName,
    int Quantity,
    Dictionary<string, object?> Configuration,
    string? Notes
);

public static class AgreementMapper
{
    public static AgreementDto ToDto(FlewClick.Domain.Entities.Agreement a, List<AgreementDeliverableDto> deliverables) =>
        new(a.Id, a.BookingRequestId, a.Version, a.PackageSnapshot,
            a.EventDate, a.EventLocation, a.EventDescription, a.TotalPrice,
            a.Terms, a.Conditions, a.Notes, a.Status, deliverables, a.CreatedAtUtc);

    public static AgreementDeliverableDto ToDto(FlewClick.Domain.Entities.AgreementDeliverable d) =>
        new(d.Id, d.DeliverableName, d.Quantity, new Dictionary<string, object?>(d.Configuration), d.Notes);
}
