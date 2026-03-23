using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPackageRepository
{
    Task<Package?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Package>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(Package package, CancellationToken ct = default);
    Task UpdateAsync(Package package, CancellationToken ct = default);
    Task RemoveAsync(Package package, CancellationToken ct = default);
}
