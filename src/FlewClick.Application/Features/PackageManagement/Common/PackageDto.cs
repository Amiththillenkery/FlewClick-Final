using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.PackageManagement.Common;

public record PackageDto(
    Guid Id,
    Guid ProfessionalProfileId,
    string Name,
    string? Description,
    PackageType PackageType,
    CoverageType? CoverageType,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
