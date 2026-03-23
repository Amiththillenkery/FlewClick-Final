using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.PackageManagement.DeletePackage;

public record DeletePackageCommand(Guid PackageId) : IRequest;

public class DeletePackageHandler(IPackageRepository packageRepository)
    : IRequestHandler<DeletePackageCommand>
{
    public async Task Handle(DeletePackageCommand request, CancellationToken ct)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId, ct)
            ?? throw new EntityNotFoundException("Package", request.PackageId);

        await packageRepository.RemoveAsync(package, ct);
    }
}
