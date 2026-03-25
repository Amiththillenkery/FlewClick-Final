using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IBookingStatusHistoryRepository
{
    Task<List<BookingStatusHistory>> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task AddAsync(BookingStatusHistory history, CancellationToken ct = default);
}
