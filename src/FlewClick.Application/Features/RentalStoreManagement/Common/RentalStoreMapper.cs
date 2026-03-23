using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.RentalStoreManagement.Common;

public static class RentalStoreMapper
{
    public static RentalStoreDto ToDto(RentalStore store) =>
        new(
            store.Id,
            store.ProfessionalProfileId,
            store.StoreName,
            store.Description,
            store.Policies,
            store.IsActive,
            store.CreatedAtUtc,
            store.UpdatedAtUtc
        );
}
