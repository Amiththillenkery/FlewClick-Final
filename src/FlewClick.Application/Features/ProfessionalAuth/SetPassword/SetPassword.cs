using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalAuth.SetPassword;

public record SetPasswordCommand(Guid AppUserId, string Password) : IRequest;

public class SetPasswordValidator : AbstractValidator<SetPasswordCommand>
{
    public SetPasswordValidator()
    {
        RuleFor(x => x.AppUserId).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[^\w\s]").WithMessage("Password must contain at least one special character.");
    }
}

public class SetPasswordHandler(
    IAppUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<SetPasswordCommand>
{
    public async Task Handle(SetPasswordCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", request.AppUserId);

        var hash = passwordHasher.Hash(request.Password);
        user.UpdatePassword(hash);
        await userRepository.UpdateAsync(user, ct);
    }
}
