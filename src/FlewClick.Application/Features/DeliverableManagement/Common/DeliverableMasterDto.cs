using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.DeliverableManagement.Common;

public record DeliverableMasterDto(
    Guid Id,
    ProfessionalRole RoleType,
    string Name,
    string? Description,
    DeliverableCategory Category,
    Dictionary<string, object?> ConfigurableAttributes,
    bool IsActive
);

public record PackageDeliverableDto(
    Guid Id,
    Guid PackageId,
    Guid DeliverableMasterId,
    string DeliverableName,
    int Quantity,
    Dictionary<string, object?> Configuration,
    string? Notes
);
