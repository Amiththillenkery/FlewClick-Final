using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IRentalProductImageRepository
{
    Task<RentalProductImage?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<RentalProductImage>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task AddAsync(RentalProductImage image, CancellationToken ct = default);
    Task RemoveAsync(RentalProductImage image, CancellationToken ct = default);
}
