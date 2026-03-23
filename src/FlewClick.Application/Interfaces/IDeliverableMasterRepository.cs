using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface IDeliverableMasterRepository
{
    Task<DeliverableMaster?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<DeliverableMaster>> GetByRoleAsync(ProfessionalRole role, CancellationToken ct = default);
    Task<IReadOnlyList<DeliverableMaster>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(DeliverableMaster deliverable, CancellationToken ct = default);
    Task UpdateAsync(DeliverableMaster deliverable, CancellationToken ct = default);
}
