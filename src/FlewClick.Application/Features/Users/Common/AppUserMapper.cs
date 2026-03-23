using FlewClick.Domain.Entities;

namespace FlewClick.Application.Features.Users.Common;

public static class AppUserMapper
{
    public static AppUserDto ToDto(AppUser user) =>
        new(
            user.Id,
            user.FullName,
            user.Email,
            user.Phone,
            user.UserType,
            user.ProfessionalRoles,
            user.IsActive,
            user.CreatedAtUtc,
            user.UpdatedAtUtc
        );
}
