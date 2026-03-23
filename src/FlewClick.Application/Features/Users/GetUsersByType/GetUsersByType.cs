using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Application.Features.Users.GetUsersByType;

public record GetUsersByTypeQuery(UserType UserType) : IRequest<IReadOnlyList<AppUserDto>>;

public class GetUsersByTypeHandler(IAppUserRepository repository)
    : IRequestHandler<GetUsersByTypeQuery, IReadOnlyList<AppUserDto>>
{
    public async Task<IReadOnlyList<AppUserDto>> Handle(GetUsersByTypeQuery request, CancellationToken ct)
    {
        var users = await repository.GetByUserTypeAsync(request.UserType, ct);
        return users.Select(AppUserMapper.ToDto).ToList();
    }
}
