using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IPortfolioItemRepository
{
    Task<List<PortfolioItem>> GetByProfileIdAsync(Guid profileId, bool visibleOnly = false, CancellationToken ct = default);
    Task<PortfolioItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PortfolioItem?> GetByInstagramMediaIdAsync(Guid profileId, string instagramMediaId, CancellationToken ct = default);
    Task AddAsync(PortfolioItem item, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<PortfolioItem> items, CancellationToken ct = default);
    Task UpdateAsync(PortfolioItem item, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<PortfolioItem> items, CancellationToken ct = default);
    Task RemoveAsync(PortfolioItem item, CancellationToken ct = default);
    Task RemoveByProfileIdAsync(Guid profileId, CancellationToken ct = default);
}
