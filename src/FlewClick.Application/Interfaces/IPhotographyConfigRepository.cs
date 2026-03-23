using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPhotographyConfigRepository
{
    Task<PhotographyConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(PhotographyConfig config, CancellationToken ct = default);
    Task UpdateAsync(PhotographyConfig config, CancellationToken ct = default);
}
