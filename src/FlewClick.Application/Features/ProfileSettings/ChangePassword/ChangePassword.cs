using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfileSettings.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}

public class ChangePasswordHandler(IAppUserRepository repository)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await repository.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException("AppUser", request.UserId);

        // Basic check for existing password if it is set.
        if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash != request.CurrentPassword)
        {
            throw new DomainException("Current password is not correct.");
        }

        user.UpdatePassword(request.NewPassword);
        await repository.UpdateAsync(user, ct);
    }
}
