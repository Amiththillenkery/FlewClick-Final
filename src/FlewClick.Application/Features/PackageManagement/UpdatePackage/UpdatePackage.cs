using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.UpdatePackage;

public record UpdatePackageCommand(
    Guid PackageId,
    string Name,
    PackageType PackageType,
    string? Description,
    CoverageType? CoverageType
) : IRequest<PackageDto>;

public class UpdatePackageValidator : AbstractValidator<UpdatePackageCommand>
{
    public UpdatePackageValidator()
    {
        RuleFor(x => x.PackageId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PackageType).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.CoverageType).IsInEnum().When(x => x.CoverageType.HasValue);
    }
}

public class UpdatePackageHandler(IPackageRepository packageRepository)
    : IRequestHandler<UpdatePackageCommand, PackageDto>
{
    public async Task<PackageDto> Handle(UpdatePackageCommand request, CancellationToken ct)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        package.Update(request.Name, request.Description, request.PackageType, request.CoverageType);
        await packageRepository.UpdateAsync(package, ct);
        return PackageMapper.ToDto(package);
    }
}
