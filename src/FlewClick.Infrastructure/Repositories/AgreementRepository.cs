using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class AgreementRepository(FlewClickDbContext context) : IAgreementRepository
{
    public async Task<Agreement?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Agreements.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<List<Agreement>> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default) =>
        await context.Agreements
            .Where(a => a.BookingRequestId == bookingRequestId)
            .OrderByDescending(a => a.Version)
            .ToListAsync(ct);

    public async Task<Agreement?> GetLatestByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default) =>
        await context.Agreements
            .Where(a => a.BookingRequestId == bookingRequestId)
            .OrderByDescending(a => a.Version)
            .FirstOrDefaultAsync(ct);

    public async Task<int> GetNextVersionAsync(Guid bookingRequestId, CancellationToken ct = default)
    {
        var count = await context.Agreements.CountAsync(a => a.BookingRequestId == bookingRequestId, ct);
        return count + 1;
    }

    public async Task AddAsync(Agreement agreement, CancellationToken ct = default)
    {
        await context.Agreements.AddAsync(agreement, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Agreement agreement, CancellationToken ct = default)
    {
        context.Agreements.Update(agreement);
        await context.SaveChangesAsync(ct);
    }
}
