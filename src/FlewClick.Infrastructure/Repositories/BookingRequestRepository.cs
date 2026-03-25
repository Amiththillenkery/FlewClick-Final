using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class BookingRequestRepository(FlewClickDbContext context) : IBookingRequestRepository
{
    public async Task<BookingRequest?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.BookingRequests.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<List<BookingRequest>> GetByConsumerIdAsync(Guid consumerId, CancellationToken ct = default) =>
        await context.BookingRequests
            .Where(b => b.ConsumerId == consumerId)
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<List<BookingRequest>> GetByProfessionalProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.BookingRequests
            .Where(b => b.ProfessionalProfileId == profileId)
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<List<BookingRequest>> GetByProfileIdAndStatusAsync(Guid profileId, BookingStatus status, CancellationToken ct = default) =>
        await context.BookingRequests
            .Where(b => b.ProfessionalProfileId == profileId && b.Status == status)
            .ToListAsync(ct);

    public async Task AddAsync(BookingRequest booking, CancellationToken ct = default)
    {
        await context.BookingRequests.AddAsync(booking, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(BookingRequest booking, CancellationToken ct = default)
    {
        context.BookingRequests.Update(booking);
        await context.SaveChangesAsync(ct);
    }
}
