using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<AppUser>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AppUser>> GetByUserTypeAsync(UserType userType, CancellationToken ct = default);
    Task<IReadOnlyList<AppUser>> GetByProfessionalRoleAsync(ProfessionalRole role, CancellationToken ct = default);
    Task AddAsync(AppUser user, CancellationToken ct = default);
    Task UpdateAsync(AppUser user, CancellationToken ct = default);
    Task RemoveAsync(AppUser user, CancellationToken ct = default);
}
