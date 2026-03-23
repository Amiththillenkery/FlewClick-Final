using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class RentalEquipmentRepository(FlewClickDbContext context) : IRentalEquipmentRepository
{
    public async Task<RentalEquipment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.RentalEquipments.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<RentalEquipment>> GetByProfileIdAsync(Guid profileId, CancellationToken ct = default) =>
        await context.RentalEquipments
            .Where(e => e.ProfessionalProfileId == profileId)
            .OrderBy(e => e.EquipmentName)
            .ToListAsync(ct);

    public async Task AddAsync(RentalEquipment equipment, CancellationToken ct = default)
    {
        await context.RentalEquipments.AddAsync(equipment, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RentalEquipment equipment, CancellationToken ct = default)
    {
        context.RentalEquipments.Update(equipment);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(RentalEquipment equipment, CancellationToken ct = default)
    {
        context.RentalEquipments.Remove(equipment);
        await context.SaveChangesAsync(ct);
    }
}
