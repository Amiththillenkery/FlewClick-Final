using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IInstagramConnectionRepository
{
    Task<InstagramConnection?> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task<InstagramConnection?> GetByInstagramUserIdAsync(string instagramUserId, CancellationToken ct = default);
    Task AddAsync(InstagramConnection connection, CancellationToken ct = default);
    Task UpdateAsync(InstagramConnection connection, CancellationToken ct = default);
    Task RemoveAsync(InstagramConnection connection, CancellationToken ct = default);
}
