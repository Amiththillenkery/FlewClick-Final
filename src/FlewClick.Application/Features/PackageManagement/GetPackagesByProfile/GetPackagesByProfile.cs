using FlewClick.Application.Features.PackageManagement.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.GetPackagesByProfile;

public record GetPackagesByProfileQuery(Guid ProfileId) : IRequest<IReadOnlyList<PackageDto>>;

public class GetPackagesByProfileHandler(IPackageRepository packageRepository)
    : IRequestHandler<GetPackagesByProfileQuery, IReadOnlyList<PackageDto>>
{
    public async Task<IReadOnlyList<PackageDto>> Handle(GetPackagesByProfileQuery request, CancellationToken ct)
    {
        var packages = await packageRepository.GetByProfileIdAsync(request.ProfileId, ct);
        return packages.Select(PackageMapper.ToDto).ToList();
    }
}
