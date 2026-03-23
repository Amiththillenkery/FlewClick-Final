using FlewClick.Application.Features.DeliverableManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.DeliverableManagement.AddDeliverableToPackage;

public record AddDeliverableToPackageCommand(
    Guid PackageId,
    Guid DeliverableMasterId,
    int Quantity,
    Dictionary<string, object?>? Configuration,
    string? Notes
) : IRequest<PackageDeliverableDto>;

public class AddDeliverableToPackageValidator : AbstractValidator<AddDeliverableToPackageCommand>
{
    public AddDeliverableToPackageValidator()
    {
        RuleFor(x => x.PackageId).NotEmpty();
        RuleFor(x => x.DeliverableMasterId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(500).When(x => x.Notes is not null);
    }
}

public class AddDeliverableToPackageHandler(
    IPackageRepository packageRepository,
    IDeliverableMasterRepository masterRepository,
    IPackageDeliverableRepository deliverableRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<AddDeliverableToPackageCommand, PackageDeliverableDto>
{
    public async Task<PackageDeliverableDto> Handle(AddDeliverableToPackageCommand request, CancellationToken ct)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        var master = await masterRepository.GetByIdAsync(request.DeliverableMasterId, ct)
            ?? throw new EntityNotFoundException("DeliverableMaster", request.DeliverableMasterId);

        if (!master.IsActive)
            throw new DomainException("This deliverable is no longer available.");

        var profile = await profileRepository.GetByIdAsync(package.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", package.ProfessionalProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (!user.HasRole(master.RoleType))
            throw new DomainException($"Deliverable '{master.Name}' is not available for your professional role.");

        var existing = await deliverableRepository.GetByPackageAndMasterIdAsync(request.PackageId, request.DeliverableMasterId, ct);
        if (existing is not null)
            throw new DomainException($"Deliverable '{master.Name}' is already added to this package. Use update instead.");

        var deliverable = PackageDeliverable.Create(
            request.PackageId, request.DeliverableMasterId,
            request.Quantity, request.Configuration, request.Notes);

        await deliverableRepository.AddAsync(deliverable, ct);
        return DeliverableMapper.ToDto(deliverable, master);
    }
}
