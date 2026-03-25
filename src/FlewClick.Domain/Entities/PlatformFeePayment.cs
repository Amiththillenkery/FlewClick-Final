using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class PlatformFeePayment : Entity
{
    public Guid BookingRequestId { get; private init; }
    public Guid ProfessionalProfileId { get; private init; }
    public decimal AgreementAmount { get; private init; }
    public decimal FeePercentage { get; private init; }
    public decimal FeeAmount { get; private init; }
    public PaymentStatus Status { get; private set; }
    public string? RazorpayOrderId { get; private set; }
    public string? RazorpayPaymentId { get; private set; }
    public string? RazorpaySignature { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTime DueDate { get; private init; }

    private PlatformFeePayment() { }

    public static PlatformFeePayment Create(
        Guid bookingRequestId, Guid professionalProfileId,
        decimal agreementAmount, decimal feePercentage = 3.0m, int gracePeriodDays = 15)
    {
        if (bookingRequestId == Guid.Empty) throw new DomainException("Booking request ID is required.");
        if (professionalProfileId == Guid.Empty) throw new DomainException("Professional profile ID is required.");
        if (agreementAmount <= 0) throw new DomainException("Agreement amount must be positive.");
        if (feePercentage <= 0 || feePercentage > 100) throw new DomainException("Fee percentage must be between 0 and 100.");

        return new PlatformFeePayment
        {
            BookingRequestId = bookingRequestId,
            ProfessionalProfileId = professionalProfileId,
            AgreementAmount = agreementAmount,
            FeePercentage = feePercentage,
            FeeAmount = Math.Round(agreementAmount * feePercentage / 100m, 2),
            Status = PaymentStatus.Pending,
            DueDate = DateTime.UtcNow.AddDays(gracePeriodDays)
        };
    }

    public void SetRazorpayOrderId(string orderId)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException($"Cannot set order ID when status is '{Status}'.");
        RazorpayOrderId = orderId;
        Status = PaymentStatus.Processing;
        Touch();
    }

    public void MarkCompleted(string paymentId, string signature)
    {
        if (Status != PaymentStatus.Processing)
            throw new DomainException($"Cannot mark completed when status is '{Status}'.");
        RazorpayPaymentId = paymentId;
        RazorpaySignature = signature;
        Status = PaymentStatus.Completed;
        PaidAt = DateTime.UtcNow;
        Touch();
    }

    public void MarkFailed(string reason)
    {
        if (Status != PaymentStatus.Processing)
            throw new DomainException($"Cannot mark failed when status is '{Status}'.");
        FailureReason = reason;
        Status = PaymentStatus.Failed;
        Touch();
    }
}
