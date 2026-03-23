using FlewClick.Application.Features.ProfessionalRegistration.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.GetRegistrationStatus;

public record GetRegistrationStatusQuery(Guid ProfileId) : IRequest<RegistrationStatusDto>;

public class GetRegistrationStatusHandler(
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IPhotographyConfigRepository photographyRepo,
    IEditingConfigRepository editingRepo,
    IDroneConfigRepository droneRepo,
    IRentalEquipmentRepository rentalRepo)
    : IRequestHandler<GetRegistrationStatusQuery, RegistrationStatusDto>
{
    public async Task<RegistrationStatusDto> Handle(GetRegistrationStatusQuery request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        var profileComplete = profile.Bio is not null || profile.Location is not null;

        var configComplete = user.ProfessionalRole switch
        {
            ProfessionalRole.PhotographerVideographer =>
                await photographyRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.Editor =>
                await editingRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.DroneOwner =>
                await droneRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.DigitalRental =>
                (await rentalRepo.GetByProfileIdAsync(profile.Id, ct)).Count > 0,
            _ => false
        };

        return new RegistrationStatusDto(
            profile.Id,
            user.ProfessionalRole!.Value,
            profileComplete,
            configComplete,
            profile.IsRegistrationComplete);
    }
}
