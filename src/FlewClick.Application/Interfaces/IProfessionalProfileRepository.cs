using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IProfessionalProfileRepository
{
    Task<ProfessionalProfile?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProfessionalProfile?> GetByAppUserIdAsync(Guid appUserId, CancellationToken ct = default);
    Task AddAsync(ProfessionalProfile profile, CancellationToken ct = default);
    Task UpdateAsync(ProfessionalProfile profile, CancellationToken ct = default);
}
