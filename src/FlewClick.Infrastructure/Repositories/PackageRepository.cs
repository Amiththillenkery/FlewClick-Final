using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PackageRepository(FlewClickDbContext context) : IPackageRepository
{
    public async Task<Package?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Packages.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Package>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.Packages
            .Where(p => p.ProfessionalProfileId == profileId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task AddAsync(Package package, CancellationToken ct = default)
    {
        await context.Packages.AddAsync(package, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Package package, CancellationToken ct = default)
    {
        context.Packages.Update(package);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Package package, CancellationToken ct = default)
    {
        context.Packages.Remove(package);
        await context.SaveChangesAsync(ct);
    }
}
