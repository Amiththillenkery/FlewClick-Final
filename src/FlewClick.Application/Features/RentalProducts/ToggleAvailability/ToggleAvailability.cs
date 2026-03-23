using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.ToggleAvailability;

public record ToggleAvailabilityCommand(Guid ProductId, bool Available) : IRequest<RentalProductDto>;

public class ToggleAvailabilityHandler(IRentalProductRepository productRepository)
    : IRequestHandler<ToggleAvailabilityCommand, RentalProductDto>
{
    public async Task<RentalProductDto> Handle(ToggleAvailabilityCommand request, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        if (request.Available) product.MarkAvailable(); else product.MarkUnavailable();

        await productRepository.UpdateAsync(product, ct);
        return RentalProductMapper.ToDto(product);
    }
}
