using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RentalStoreRepository(FlewClickDbContext context) : IRentalStoreRepository
{
    public async Task<RentalStore?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.RentalStores.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<RentalStore?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.RentalStores.FirstOrDefaultAsync(s => s.ProfessionalProfileId == profileId, ct);

    public async Task AddAsync(RentalStore store, CancellationToken ct = default)
    {
        await context.RentalStores.AddAsync(store, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RentalStore store, CancellationToken ct = default)
    {
        context.RentalStores.Update(store);
        await context.SaveChangesAsync(ct);
    }
}
