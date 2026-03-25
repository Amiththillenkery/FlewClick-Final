namespace FlewClick.Application.Features.ProfessionalAuth.Common;

public record AuthTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    ProfessionalUserDto Profile
);

public record TokenRefreshResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds
);

public record ProfessionalUserDto(
    Guid UserId,
    Guid ProfileId,
    string FullName,
    string Email,
    string? Phone,
    List<string> Roles,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    bool IsRegistrationComplete
);
