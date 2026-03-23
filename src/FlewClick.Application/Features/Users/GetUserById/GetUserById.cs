using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<AppUserDto?>;

public class GetUserByIdHandler(IAppUserRepository repository)
    : IRequestHandler<GetUserByIdQuery, AppUserDto?>
{
    public async Task<AppUserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await repository.GetByIdAsync(request.Id, ct);
        return user is null ? null : AppUserMapper.ToDto(user);
    }
}
