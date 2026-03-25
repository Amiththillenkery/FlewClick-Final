using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Browse.Common;

public record CategoryDto(ProfessionalRole Role, string Name, int ProfessionalCount);

public record ProfessionalListingDto(
    Guid ProfileId,
    string FullName,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    List<ProfessionalRole> ProfessionalRoles,
    DateTime CreatedAtUtc
);
