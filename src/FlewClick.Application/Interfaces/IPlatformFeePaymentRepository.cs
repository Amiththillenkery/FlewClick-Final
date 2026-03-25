using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPlatformFeePaymentRepository
{
    Task<PlatformFeePayment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PlatformFeePayment?> GetByRazorpayOrderIdAsync(string razorpayOrderId, CancellationToken ct = default);
    Task<PlatformFeePayment?> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task<List<PlatformFeePayment>> GetOutstandingByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task<bool> HasOutstandingFeesAsync(Guid profileId, CancellationToken ct = default);
    Task<List<PlatformFeePayment>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(PlatformFeePayment payment, CancellationToken ct = default);
    Task UpdateAsync(PlatformFeePayment payment, CancellationToken ct = default);
}
