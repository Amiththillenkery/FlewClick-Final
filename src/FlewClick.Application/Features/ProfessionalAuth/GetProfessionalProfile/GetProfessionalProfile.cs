using FlewClick.Application.Features.ProfessionalAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalAuth.GetProfessionalProfile;

public record GetProfessionalProfileQuery(Guid UserId) : IRequest<ProfessionalUserDto>;

public class GetProfessionalProfileValidator : AbstractValidator<GetProfessionalProfileQuery>
{
    public GetProfessionalProfileValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetProfessionalProfileHandler(
    IAppUserRepository userRepository,
    IProfessionalProfileRepository profileRepository)
    : IRequestHandler<GetProfessionalProfileQuery, ProfessionalUserDto>
{
    public async Task<ProfessionalUserDto> Handle(GetProfessionalProfileQuery request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException("AppUser", request.UserId);

        var profile = await profileRepository.GetByAppUserIdAsync(user.Id, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", user.Id);

        return new ProfessionalUserDto(
            user.Id,
            profile.Id,
            user.FullName,
            user.Email,
            user.Phone,
            user.ProfessionalRoles.Select(r => r.ToString()).ToList(),
            profile.Bio,
            profile.Location,
            profile.YearsOfExperience,
            profile.IsRegistrationComplete
        );
    }
}
