using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Users.UpdateUserProfile;

public record UpdateUserProfileCommand(
    Guid Id,
    string FullName,
    string? Phone
) : IRequest;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
    }
}

public class UpdateUserProfileHandler(IAppUserRepository repository)
    : IRequestHandler<UpdateUserProfileCommand>
{
    public async Task Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var user = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new EntityNotFoundException("AppUser", request.Id);

        user.UpdateProfile(request.FullName, request.Phone);
        await repository.UpdateAsync(user, ct);
    }
}
