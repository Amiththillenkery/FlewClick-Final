using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PlatformFeePaymentRepository(FlewClickDbContext context) : IPlatformFeePaymentRepository
{
    public async Task<PlatformFeePayment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.PlatformFeePayments.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<PlatformFeePayment?> GetByRazorpayOrderIdAsync(string razorpayOrderId, CancellationToken ct = default) =>
        await context.PlatformFeePayments.FirstOrDefaultAsync(p => p.RazorpayOrderId == razorpayOrderId, ct);

    public async Task<PlatformFeePayment?> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default) =>
        await context.PlatformFeePayments.FirstOrDefaultAsync(p => p.BookingRequestId == bookingRequestId, ct);

    public async Task<List<PlatformFeePayment>> GetOutstandingByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.PlatformFeePayments
            .Where(p => p.ProfessionalProfileId == profileId
                && (p.Status == PaymentStatus.Pending || p.Status == PaymentStatus.Processing))
            .ToListAsync(ct);

    public async Task<bool> HasOutstandingFeesAsync(Guid profileId, CancellationToken ct = default) =>
        await context.PlatformFeePayments.AnyAsync(
            p => p.ProfessionalProfileId == profileId
                && (p.Status == PaymentStatus.Pending || p.Status == PaymentStatus.Processing),
            ct);

    public async Task<List<PlatformFeePayment>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.PlatformFeePayments
            .Where(p => p.ProfessionalProfileId == profileId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task AddAsync(PlatformFeePayment payment, CancellationToken ct = default)
    {
        await context.PlatformFeePayments.AddAsync(payment, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PlatformFeePayment payment, CancellationToken ct = default)
    {
        context.PlatformFeePayments.Update(payment);
        await context.SaveChangesAsync(ct);
    }
}
