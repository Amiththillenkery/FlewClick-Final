using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RefreshTokenRepository(FlewClickDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
        => await context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token, ct);

    public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken ct)
        => await context.RefreshTokens
            .Where(r => r.AppUserId == userId && !r.IsRevoked)
            .ToListAsync(ct);

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct)
    {
        await context.RefreshTokens.AddAsync(refreshToken, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken ct)
    {
        context.RefreshTokens.Update(refreshToken);
        await context.SaveChangesAsync(ct);
    }
}
