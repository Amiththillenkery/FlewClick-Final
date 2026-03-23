using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RentalProductPricingRepository(FlewClickDbContext context) : IRentalProductPricingRepository
{
    public async Task<RentalProductPricing?> GetByProductIdAsync(Guid productId, CancellationToken ct = default) =>
        await context.RentalProductPricings.FirstOrDefaultAsync(p => p.RentalProductId == productId, ct);

    public async Task AddAsync(RentalProductPricing pricing, CancellationToken ct = default)
    {
        await context.RentalProductPricings.AddAsync(pricing, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RentalProductPricing pricing, CancellationToken ct = default)
    {
        context.RentalProductPricings.Update(pricing);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(RentalProductPricing pricing, CancellationToken ct = default)
    {
        context.RentalProductPricings.Remove(pricing);
        await context.SaveChangesAsync(ct);
    }
}
