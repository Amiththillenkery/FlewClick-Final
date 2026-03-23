using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.PackageManagement.Common;

public static class PackageMapper
{
    public static PackageDto ToDto(Package package) =>
        new(
            package.Id,
            package.ProfessionalProfileId,
            package.Name,
            package.Description,
            package.PackageType,
            package.CoverageType,
            package.IsActive,
            package.CreatedAtUtc,
            package.UpdatedAtUtc
        );
}
