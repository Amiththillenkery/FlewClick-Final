using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class ProfessionalProfileRepository(FlewClickDbContext context) : IProfessionalProfileRepository
{
    public async Task<ProfessionalProfile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.ProfessionalProfiles.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<ProfessionalProfile?> GetByAppUserIdAsync(Guid appUserId, CancellationToken ct = default) =>
        await context.ProfessionalProfiles.FirstOrDefaultAsync(p => p.AppUserId == appUserId, ct);

    public async Task AddAsync(ProfessionalProfile profile, CancellationToken ct = default)
    {
        await context.ProfessionalProfiles.AddAsync(profile, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ProfessionalProfile profile, CancellationToken ct = default)
    {
        context.ProfessionalProfiles.Update(profile);
        await context.SaveChangesAsync(ct);
    }
}
