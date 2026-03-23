using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPackagePricingRepository
{
    Task<PackagePricing?> GetByPackageIdAsync(Guid packageId, CancellationToken ct = default);
    Task AddAsync(PackagePricing pricing, CancellationToken ct = default);
    Task UpdateAsync(PackagePricing pricing, CancellationToken ct = default);
    Task RemoveAsync(PackagePricing pricing, CancellationToken ct = default);
}
