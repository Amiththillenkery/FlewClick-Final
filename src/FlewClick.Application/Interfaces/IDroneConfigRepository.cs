using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IDroneConfigRepository
{
    Task<DroneConfig?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(DroneConfig config, CancellationToken ct = default);
    Task UpdateAsync(DroneConfig config, CancellationToken ct = default);
}
