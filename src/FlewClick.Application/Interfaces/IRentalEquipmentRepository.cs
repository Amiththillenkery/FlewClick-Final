using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IRentalEquipmentRepository
{
    Task<RentalEquipment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<RentalEquipment>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task AddAsync(RentalEquipment equipment, CancellationToken ct = default);
    Task UpdateAsync(RentalEquipment equipment, CancellationToken ct = default);
    Task RemoveAsync(RentalEquipment equipment, CancellationToken ct = default);
}
