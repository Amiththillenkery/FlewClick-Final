using FlewClick.Application.Features.Shortlist.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Shortlist.GetSavedProfessionals;

public record GetSavedProfessionalsQuery(Guid ConsumerId) : IRequest<List<SavedProfessionalDto>>;

public class GetSavedProfessionalsValidator : AbstractValidator<GetSavedProfessionalsQuery>
{
    public GetSavedProfessionalsValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}

public class GetSavedProfessionalsHandler(
    ISavedProfessionalRepository savedRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<GetSavedProfessionalsQuery, List<SavedProfessionalDto>>
{
    public async Task<List<SavedProfessionalDto>> Handle(GetSavedProfessionalsQuery request, CancellationToken ct)
    {
        var savedList = await savedRepository.GetByConsumerIdAsync(request.ConsumerId, ct);
        var result = new List<SavedProfessionalDto>();

        foreach (var saved in savedList)
        {
            var profile = await profileRepository.GetByIdAsync(saved.ProfessionalProfileId, ct);
            if (profile is null) continue;

            var user = await userRepository.GetByIdAsync(profile.AppUserId, ct);
            if (user is null) continue;

            result.Add(new SavedProfessionalDto(
                saved.Id,
                saved.ConsumerId,
                saved.ProfessionalProfileId,
                user.FullName,
                profile.Location,
                profile.HourlyRate,
                user.ProfessionalRoles,
                saved.CreatedAtUtc
            ));
        }

        return result;
    }
}
