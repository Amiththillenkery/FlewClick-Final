using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Users.Common;

public record AppUserDto(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    UserType UserType,
    ProfessionalRole? ProfessionalRole,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
