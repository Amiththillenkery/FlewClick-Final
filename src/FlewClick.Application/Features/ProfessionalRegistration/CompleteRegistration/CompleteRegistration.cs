using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.ProfessionalRegistration.CompleteRegistration;

public record CompleteRegistrationCommand(Guid ProfileId) : IRequest;

public class CompleteRegistrationHandler(
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IPhotographyConfigRepository photographyRepo,
    IEditingConfigRepository editingRepo,
    IDroneConfigRepository droneRepo,
    IRentalEquipmentRepository rentalRepo)
    : IRequestHandler<CompleteRegistrationCommand>
{
    public async Task Handle(CompleteRegistrationCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        if (profile.IsRegistrationComplete)
            throw new DomainException("Registration is already complete.");

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        var configExists = user.ProfessionalRole switch
        {
            ProfessionalRole.PhotographerVideographer =>
                await photographyRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.Editor =>
                await editingRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.DroneOwner =>
                await droneRepo.GetByProfileIdAsync(profile.Id, ct) is not null,
            ProfessionalRole.DigitalRental =>
                (await rentalRepo.GetByProfileIdAsync(profile.Id, ct)).Count > 0,
            _ => throw new DomainException("Unknown professional role.")
        };

        if (!configExists)
            throw new DomainException($"Professional configuration for {user.ProfessionalRole} must be completed before finishing registration.");

        profile.MarkRegistrationComplete();
        await profileRepository.UpdateAsync(profile, ct);
    }
}
