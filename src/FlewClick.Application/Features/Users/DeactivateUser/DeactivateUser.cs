using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.Users.DeactivateUser;

public record DeactivateUserCommand(Guid Id) : IRequest;

public class DeactivateUserHandler(IAppUserRepository repository)
    : IRequestHandler<DeactivateUserCommand>
{
    public async Task Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        var user = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException("AppUser", request.Id);

        user.Deactivate();
        await repository.UpdateAsync(user, ct);
    }
}
