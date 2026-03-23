using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.TogglePackageStatus;

public record TogglePackageStatusCommand(Guid PackageId, bool Activate) : IRequest<PackageDto>;

public class TogglePackageStatusHandler(IPackageRepository packageRepository)
    : IRequestHandler<TogglePackageStatusCommand, PackageDto>
{
    public async Task<PackageDto> Handle(TogglePackageStatusCommand request, CancellationToken ct)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        if (request.Activate) package.Activate(); else package.Deactivate();

        await packageRepository.UpdateAsync(package, ct);
        return PackageMapper.ToDto(package);
    }
}
