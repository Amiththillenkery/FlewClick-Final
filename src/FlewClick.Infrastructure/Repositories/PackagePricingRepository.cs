using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PackagePricingRepository(FlewClickDbContext context) : IPackagePricingRepository
{
    public async Task<PackagePricing?> GetByPackageIdAsync(Guid packageId, CancellationToken ct = default) =>
        await context.PackagePricings.FirstOrDefaultAsync(p => p.PackageId == packageId, ct);

    public async Task AddAsync(PackagePricing pricing, CancellationToken ct = default)
    {
        await context.PackagePricings.AddAsync(pricing, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PackagePricing pricing, CancellationToken ct = default)
    {
        context.PackagePricings.Update(pricing);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(PackagePricing pricing, CancellationToken ct = default)
    {
        context.PackagePricings.Remove(pricing);
        await context.SaveChangesAsync(ct);
    }
}
