using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.PlatformFee.Common;

public record PlatformFeeDto(
    Guid Id,
    Guid BookingRequestId,
    Guid ProfessionalProfileId,
    decimal AgreementAmount,
    decimal FeePercentage,
    decimal FeeAmount,
    PaymentStatus Status,
    string? RazorpayOrderId,
    DateTime? PaidAt,
    DateTime DueDate,
    DateTime CreatedAtUtc);

public record OutstandingFeesDto(
    bool IsBlocked,
    decimal TotalOutstanding,
    List<PlatformFeeDto> Fees);

public static class PlatformFeeMapper
{
    public static PlatformFeeDto ToDto(PlatformFeePayment p) =>
        new(p.Id, p.BookingRequestId, p.ProfessionalProfileId,
            p.AgreementAmount, p.FeePercentage, p.FeeAmount,
            p.Status, p.RazorpayOrderId, p.PaidAt, p.DueDate, p.CreatedAtUtc);
}
