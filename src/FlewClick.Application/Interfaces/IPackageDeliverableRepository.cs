using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPackageDeliverableRepository
{
    Task<PackageDeliverable?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<PackageDeliverable>> GetByPackageIdAsync(Guid packageId, CancellationToken ct = default);
    Task<PackageDeliverable?> GetByPackageAndMasterIdAsync(Guid packageId, Guid masterDeliverableId, CancellationToken ct = default);
    Task AddAsync(PackageDeliverable deliverable, CancellationToken ct = default);
    Task UpdateAsync(PackageDeliverable deliverable, CancellationToken ct = default);
    Task RemoveAsync(PackageDeliverable deliverable, CancellationToken ct = default);
}
