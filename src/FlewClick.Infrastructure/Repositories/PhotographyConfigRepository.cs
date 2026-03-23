using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class PhotographyConfigRepository(FlewClickDbContext context) : IPhotographyConfigRepository
{
    public async Task<PhotographyConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.PhotographyConfigs.FirstOrDefaultAsync(c => c.ProfessionalProfileId == profileId, ct);

    public async Task AddAsync(PhotographyConfig config, CancellationToken ct = default)
    {
        await context.PhotographyConfigs.AddAsync(config, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PhotographyConfig config, CancellationToken ct = default)
    {
        context.PhotographyConfigs.Update(config);
        await context.SaveChangesAsync(ct);
    }
}
