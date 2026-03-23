using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IEditingConfigRepository
{
    Task<EditingConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(EditingConfig config, CancellationToken ct = default);
    Task UpdateAsync(EditingConfig config, CancellationToken ct = default);
}
