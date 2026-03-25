using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface IBookingRequestRepository
{
    Task<BookingRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<BookingRequest>> GetByConsumerIdAsync(Guid consumerId, CancellationToken ct = default);
    Task<List<BookingRequest>> GetByProfessionalProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task<List<BookingRequest>> GetByProfileIdAndStatusAsync(Guid profileId, BookingStatus status, CancellationToken ct = default);
    Task AddAsync(BookingRequest booking, CancellationToken ct = default);
    Task UpdateAsync(BookingRequest booking, CancellationToken ct = default);
}
