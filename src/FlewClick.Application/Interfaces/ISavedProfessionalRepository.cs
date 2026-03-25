using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface ISavedProfessionalRepository
{
    Task<List<SavedProfessional>> GetByConsumerIdAsync(Guid consumerId, CancellationToken ct = default);
    Task<SavedProfessional?> GetByConsumerAndProfileAsync(Guid consumerId, Guid profileId, CancellationToken ct = default);
    Task AddAsync(SavedProfessional saved, CancellationToken ct = default);
    Task RemoveAsync(SavedProfessional saved, CancellationToken ct = default);
}
