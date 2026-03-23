using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest;

public class DeleteProductHandler(IRentalProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        await productRepository.RemoveAsync(product, ct);
    }
}
