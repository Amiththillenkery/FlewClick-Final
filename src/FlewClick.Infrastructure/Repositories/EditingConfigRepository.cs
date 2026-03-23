using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class EditingConfigRepository(FlewClickDbContext context) : IEditingConfigRepository
{
    public async Task<EditingConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.EditingConfigs.FirstOrDefaultAsync(c => c.ProfessionalProfileId == profileId, ct);

    public async Task AddAsync(EditingConfig config, CancellationToken ct = default)
    {
        await context.EditingConfigs.AddAsync(config, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(EditingConfig config, CancellationToken ct = default)
    {
        context.EditingConfigs.Update(config);
        await context.SaveChangesAsync(ct);
    }
}
