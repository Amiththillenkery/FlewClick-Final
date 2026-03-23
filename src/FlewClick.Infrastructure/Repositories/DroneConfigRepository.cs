using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class DroneConfigRepository(FlewClickDbContext context) : IDroneConfigRepository
{
    public async Task<DroneConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.DroneConfigs.FirstOrDefaultAsync(c => c.ProfessionalProfileId == profileId, ct);

    public async Task AddAsync(DroneConfig config, CancellationToken ct = default)
    {
        await context.DroneConfigs.AddAsync(config, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DroneConfig config, CancellationToken ct = default)
    {
        context.DroneConfigs.Update(config);
        await context.SaveChangesAsync(ct);
    }
}
