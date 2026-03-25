using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class BookingStatusHistoryRepository(FlewClickDbContext context) : IBookingStatusHistoryRepository
{
    public async Task<List<BookingStatusHistory>> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default) =>
        await context.BookingStatusHistory
            .Where(h => h.BookingRequestId == bookingRequestId)
            .OrderBy(h => h.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task AddAsync(BookingStatusHistory history, CancellationToken ct = default)
    {
        await context.BookingStatusHistory.AddAsync(history, ct);
        await context.SaveChangesAsync(ct);
    }
}
