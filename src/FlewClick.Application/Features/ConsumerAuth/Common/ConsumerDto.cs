namespace FlewClick.Application.Features.ConsumerAuth.Common;

public record ConsumerDto(
    Guid Id,
    string Phone,
    string FullName,
    string? Email,
    bool IsPhoneVerified,
    bool IsActive,
    DateTime? LastLoginAt,
    DateTime CreatedAtUtc
);

public record AuthResponseDto(
    string Token,
    ConsumerDto Consumer
);

public record ConsumerAuthTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    ConsumerDto Consumer
);

public record ConsumerTokenRefreshResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds
);
