using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IRentalStoreRepository
{
    Task<RentalStore?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RentalStore?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(RentalStore store, CancellationToken ct = default);
    Task UpdateAsync(RentalStore store, CancellationToken ct = default);
}
