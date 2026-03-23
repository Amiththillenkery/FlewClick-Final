using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class AppUserRepository(FlewClickDbContext context) : IAppUserRepository
{
    public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await context.AppUsers.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public async Task<IReadOnlyList<AppUser>> GetAllAsync(CancellationToken ct = default) =>
        await context.AppUsers.OrderBy(u => u.FullName).ToListAsync(ct);

    public async Task<IReadOnlyList<AppUser>> GetByUserTypeAsync(UserType userType, CancellationToken ct = default) =>
        await context.AppUsers
            .Where(u => u.UserType == userType)
            .OrderBy(u => u.FullName)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<AppUser>> GetByProfessionalRoleAsync(ProfessionalRole role, CancellationToken ct = default) =>
        await context.AppUsers
            .Where(u => u.UserType == UserType.ProfessionalUser && u.ProfessionalRole == role)
            .OrderBy(u => u.FullName)
            .ToListAsync(ct);

    public async Task AddAsync(AppUser user, CancellationToken ct = default)
    {
        await context.AppUsers.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(AppUser user, CancellationToken ct = default)
    {
        context.AppUsers.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(AppUser user, CancellationToken ct = default)
    {
        context.AppUsers.Remove(user);
        await context.SaveChangesAsync(ct);
    }
}
