using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RentalProductRepository(FlewClickDbContext context) : IRentalProductRepository
{
    public async Task<RentalProduct?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.RentalProducts.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<RentalProduct>> GetByStoreIdAsync(Guid storeId, CancellationToken ct = default) =>
        await context.RentalProducts
            .Where(p => p.RentalStoreId == storeId)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task AddAsync(RentalProduct product, CancellationToken ct = default)
    {
        await context.RentalProducts.AddAsync(product, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RentalProduct product, CancellationToken ct = default)
    {
        context.RentalProducts.Update(product);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(RentalProduct product, CancellationToken ct = default)
    {
        context.RentalProducts.Remove(product);
        await context.SaveChangesAsync(ct);
    }
}
