using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RentalProductImageRepository(FlewClickDbContext context) : IRentalProductImageRepository
{
    public async Task<RentalProductImage?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.RentalProductImages.FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IReadOnlyList<RentalProductImage>> GetByProductIdAsync(Guid productId, CancellationToken ct = default) =>
        await context.RentalProductImages
            .Where(i => i.RentalProductId == productId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync(ct);

    public async Task AddAsync(RentalProductImage image, CancellationToken ct = default)
    {
        await context.RentalProductImages.AddAsync(image, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(RentalProductImage image, CancellationToken ct = default)
    {
        context.RentalProductImages.Remove(image);
        await context.SaveChangesAsync(ct);
    }
}
