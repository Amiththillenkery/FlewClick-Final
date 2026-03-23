using FlewClick.Application.Features.Users.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Application.Features.Users.GetProfessionalsByRole;

public record GetProfessionalsByRoleQuery(ProfessionalRole Role) : IRequest<IReadOnlyList<AppUserDto>>;

public class GetProfessionalsByRoleHandler(IAppUserRepository repository)
    : IRequestHandler<GetProfessionalsByRoleQuery, IReadOnlyList<AppUserDto>>
{
    public async Task<IReadOnlyList<AppUserDto>> Handle(GetProfessionalsByRoleQuery request, CancellationToken ct)
    {
        var users = await repository.GetByProfessionalRoleAsync(request.Role, ct);
        return users.Select(AppUserMapper.ToDto).ToList();
    }
}
