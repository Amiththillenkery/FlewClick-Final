using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class SavedProfessionalRepository(FlewClickDbContext context) : ISavedProfessionalRepository
{
    public async Task<List<SavedProfessional>> GetByConsumerIdAsync(Guid consumerId, CancellationToken ct = default) =>
        await context.SavedProfessionals
            .Where(s => s.ConsumerId == consumerId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<SavedProfessional?> GetByConsumerAndProfileAsync(
        Guid consumerId, Guid profileId, CancellationToken ct = default) =>
        await context.SavedProfessionals
            .FirstOrDefaultAsync(s => s.ConsumerId == consumerId && s.ProfessionalProfileId == profileId, ct);

    public async Task AddAsync(SavedProfessional saved, CancellationToken ct = default)
    {
        await context.SavedProfessionals.AddAsync(saved, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(SavedProfessional saved, CancellationToken ct = default)
    {
        context.SavedProfessionals.Remove(saved);
        await context.SaveChangesAsync(ct);
    }
}
