using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IRentalProductPricingRepository
{
    Task<RentalProductPricing?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task AddAsync(RentalProductPricing pricing, CancellationToken ct = default);
    Task UpdateAsync(RentalProductPricing pricing, CancellationToken ct = default);
    Task RemoveAsync(RentalProductPricing pricing, CancellationToken ct = default);
}
