namespace FlewClick.Application.Features.ProfessionalRegistration.Common;

public record ProfessionalProfileDto(
    Guid Id,
    Guid AppUserId,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl,
    bool IsRegistrationComplete,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
