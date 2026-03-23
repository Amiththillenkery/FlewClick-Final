using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.Users.GetAllUsers;

public record GetAllUsersQuery : IRequest<IReadOnlyList<AppUserDto>>;

public class GetAllUsersHandler(IAppUserRepository repository)
    : IRequestHandler<GetAllUsersQuery, IReadOnlyList<AppUserDto>>
{
    public async Task<IReadOnlyList<AppUserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await repository.GetAllAsync(ct);
        return users.Select(AppUserMapper.ToDto).ToList();
    }
}
