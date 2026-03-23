using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PackageDeliverableRepository(FlewClickDbContext context) : IPackageDeliverableRepository
{
    public async Task<PackageDeliverable?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.PackageDeliverables.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IReadOnlyList<PackageDeliverable>> GetByPackageIdAsync(Guid packageId, CancellationToken ct = default) =>
        await context.PackageDeliverables
            .Where(d => d.PackageId == packageId)
            .OrderBy(d => d.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<PackageDeliverable?> GetByPackageAndMasterIdAsync(Guid packageId, Guid masterDeliverableId, CancellationToken ct = default) =>
        await context.PackageDeliverables
            .FirstOrDefaultAsync(d => d.PackageId == packageId && d.DeliverableMasterId == masterDeliverableId, ct);

    public async Task AddAsync(PackageDeliverable deliverable, CancellationToken ct = default)
    {
        await context.PackageDeliverables.AddAsync(deliverable, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PackageDeliverable deliverable, CancellationToken ct = default)
    {
        context.PackageDeliverables.Update(deliverable);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(PackageDeliverable deliverable, CancellationToken ct = default)
    {
        context.PackageDeliverables.Remove(deliverable);
        await context.SaveChangesAsync(ct);
    }
}
