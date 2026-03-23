namespace FlewClick.Application.Features.RentalStoreManagement.Common;

public record RentalStoreDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string StoreName,
    string? Description,
    Dictionary<string, object?> Policies,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
