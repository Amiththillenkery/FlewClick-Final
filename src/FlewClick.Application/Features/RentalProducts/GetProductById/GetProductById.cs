using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<RentalProductDetailDto>;

public class GetProductByIdHandler(
    IRentalProductRepository productRepository,
    IRentalProductImageRepository imageRepository,
    IRentalProductPricingRepository pricingRepository)
    : IRequestHandler<GetProductByIdQuery, RentalProductDetailDto>
{
    public async Task<RentalProductDetailDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        var images = await imageRepository.GetByProductIdAsync(request.ProductId, ct);
        var pricing = await pricingRepository.GetByProductIdAsync(request.ProductId, ct);

        return new RentalProductDetailDto(
            RentalProductMapper.ToDto(product),
            images.Select(RentalProductMapper.ToDto).ToList(),
            pricing is not null ? RentalProductMapper.ToDto(pricing) : null
        );
    }
}
