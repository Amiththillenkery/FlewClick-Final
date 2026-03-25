using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class InstagramConnectionRepository(FlewClickDbContext context) : IInstagramConnectionRepository
{
    public async Task<InstagramConnection?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.InstagramConnections.FirstOrDefaultAsync(c => c.ProfessionalProfileId == profileId, ct);

    public async Task<InstagramConnection?> GetByInstagramUserIdAsync(string instagramUserId, CancellationToken ct = default) =>
        await context.InstagramConnections.FirstOrDefaultAsync(c => c.InstagramUserId == instagramUserId, ct);

    public async Task AddAsync(InstagramConnection connection, CancellationToken ct = default)
    {
        await context.InstagramConnections.AddAsync(connection, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(InstagramConnection connection, CancellationToken ct = default)
    {
        context.InstagramConnections.Update(connection);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(InstagramConnection connection, CancellationToken ct = default)
    {
        context.InstagramConnections.Remove(connection);
        await context.SaveChangesAsync(ct);
    }
}
