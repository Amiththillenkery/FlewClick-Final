using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IRentalProductRepository
{
    Task<RentalProduct?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<RentalProduct>> GetByStoreIdAsync(Guid storeId, CancellationToken ct = default);
    Task AddAsync(RentalProduct product, CancellationToken ct = default);
    Task UpdateAsync(RentalProduct product, CancellationToken ct = default);
    Task RemoveAsync(RentalProduct product, CancellationToken ct = default);
}
