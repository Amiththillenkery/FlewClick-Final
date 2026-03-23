using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.GetPackageById;

public record GetPackageByIdQuery(Guid PackageId) : IRequest<PackageDto>;

public class GetPackageByIdHandler(IPackageRepository packageRepository)
    : IRequestHandler<GetPackageByIdQuery, PackageDto>
{
    public async Task<PackageDto> Handle(GetPackageByIdQuery request, CancellationToken ct)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        return PackageMapper.ToDto(package);
    }
}
