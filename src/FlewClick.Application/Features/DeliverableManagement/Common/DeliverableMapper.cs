using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.DeliverableManagement.Common;

public static class DeliverableMapper
{
    public static DeliverableMasterDto ToDto(DeliverableMaster master) =>
        new(
            master.Id,
            master.RoleType,
            master.Name,
            master.Description,
            master.Category,
            master.ConfigurableAttributes,
            master.IsActive
        );

    public static PackageDeliverableDto ToDto(PackageDeliverable pd, DeliverableMaster master) =>
        new(
            pd.Id,
            pd.PackageId,
            pd.DeliverableMasterId,
            master.Name,
            pd.Quantity,
            pd.Configuration,
            pd.Notes
        );
}
