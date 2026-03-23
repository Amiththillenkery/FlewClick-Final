using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfileSettings.UpdateAccountSettings;

public record UpdateAccountSettingsCommand(
    Guid UserId,
    string FullName,
    string? Phone
) : IRequest;

public class UpdateAccountSettingsValidator : AbstractValidator<UpdateAccountSettingsCommand>
{
    public UpdateAccountSettingsValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
    }
}

public class UpdateAccountSettingsHandler(IAppUserRepository repository)
    : IRequestHandler<UpdateAccountSettingsCommand>
{
    public async Task Handle(UpdateAccountSettingsCommand request, CancellationToken ct)
    {
        var user = await repository.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException("AppUser", request.UserId);

        user.UpdateProfile(request.FullName, request.Phone);
        await repository.UpdateAsync(user, ct);
    }
}
