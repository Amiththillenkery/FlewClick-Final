using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.ListProducts;

public record ListProductsQuery(Guid StoreId) : IRequest<IReadOnlyList<RentalProductDto>>;

public class ListProductsHandler(IRentalProductRepository productRepository)
    : IRequestHandler<ListProductsQuery, IReadOnlyList<RentalProductDto>>
{
    public async Task<IReadOnlyList<RentalProductDto>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        var products = await productRepository.GetByStoreIdAsync(request.StoreId, ct);
        return products.Select(RentalProductMapper.ToDto).ToList();
    }
}
