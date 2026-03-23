using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.RemoveProductImage;

public record RemoveProductImageCommand(Guid ImageId) : IRequest;

public class RemoveProductImageHandler(IRentalProductImageRepository imageRepository)
    : IRequestHandler<RemoveProductImageCommand>
{
    public async Task Handle(RemoveProductImageCommand request, CancellationToken ct)
    {
        var image = await imageRepository.GetByIdAsync(request.ImageId, ct)
            ?? throw new EntityNotFoundException("RentalProductImage", request.ImageId);

        await imageRepository.RemoveAsync(image, ct);
    }
}
