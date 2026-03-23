using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.CreatePackage;

public record CreatePackageCommand(
    Guid ProfileId,
    string Name,
    PackageType PackageType,
    string? Description,
    CoverageType? CoverageType
) : IRequest<PackageDto>;

public class CreatePackageValidator : AbstractValidator<CreatePackageCommand>
{
    public CreatePackageValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PackageType).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.CoverageType).IsInEnum().When(x => x.CoverageType.HasValue);
    }
}

public class CreatePackageHandler(
    IPackageRepository packageRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<CreatePackageCommand, PackageDto>
{
    public async Task<PackageDto> Handle(CreatePackageCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (user.HasRole(ProfessionalRole.DigitalRental) && user.ProfessionalRoles.Count == 1)
            throw new DomainException("Package creation is not available for Digital Rental professionals. Use the rental store instead.");

        var package = Package.Create(
            request.ProfileId, request.Name, request.PackageType,
            request.Description, request.CoverageType);

        await packageRepository.AddAsync(package, ct);
        return PackageMapper.ToDto(package);
    }
}
