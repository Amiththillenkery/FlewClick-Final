using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PortfolioItemRepository(FlewClickDbContext context) : IPortfolioItemRepository
{
    public async Task<List<PortfolioItem>> GetByProfileIdAsync(
        Guid profileId, bool visibleOnly = false, CancellationToken ct = default)
    {
        var query = context.PortfolioItems
            .Where(i => i.ProfessionalProfileId == profileId);

        if (visibleOnly)
            query = query.Where(i => i.IsVisible);

        return await query.OrderBy(i => i.DisplayOrder).ToListAsync(ct);
    }

    public async Task<PortfolioItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.PortfolioItems.FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<PortfolioItem?> GetByInstagramMediaIdAsync(
        Guid profileId, string instagramMediaId, CancellationToken ct = default) =>
        await context.PortfolioItems.FirstOrDefaultAsync(
            i => i.ProfessionalProfileId == profileId && i.InstagramMediaId == instagramMediaId, ct);

    public async Task AddAsync(PortfolioItem item, CancellationToken ct = default)
    {
        await context.PortfolioItems.AddAsync(item, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<PortfolioItem> items, CancellationToken ct = default)
    {
        await context.PortfolioItems.AddRangeAsync(items, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PortfolioItem item, CancellationToken ct = default)
    {
        context.PortfolioItems.Update(item);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateRangeAsync(IEnumerable<PortfolioItem> items, CancellationToken ct = default)
    {
        context.PortfolioItems.UpdateRange(items);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(PortfolioItem item, CancellationToken ct = default)
    {
        context.PortfolioItems.Remove(item);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveByProfileIdAsync(Guid profileId, CancellationToken ct = default)
    {
        var items = await context.PortfolioItems
            .Where(i => i.ProfessionalProfileId == profileId)
            .ToListAsync(ct);

        if (items.Count > 0)
        {
            context.PortfolioItems.RemoveRange(items);
            await context.SaveChangesAsync(ct);
        }
    }
}
