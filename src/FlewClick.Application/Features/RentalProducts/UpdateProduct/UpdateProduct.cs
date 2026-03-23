using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    ProductCondition Condition,
    string? Description,
    string? Category,
    string? Brand,
    string? Model,
    Dictionary<string, object?>? Specifications
) : IRequest<RentalProductDto>;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Condition).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.Category).MaximumLength(100).When(x => x.Category is not null);
        RuleFor(x => x.Brand).MaximumLength(100).When(x => x.Brand is not null);
        RuleFor(x => x.Model).MaximumLength(200).When(x => x.Model is not null);
    }
}

public class UpdateProductHandler(IRentalProductRepository productRepository)
    : IRequestHandler<UpdateProductCommand, RentalProductDto>
{
    public async Task<RentalProductDto> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        product.Update(request.Name, request.Description, request.Category,
            request.Brand, request.Model, request.Condition, request.Specifications);

        await productRepository.UpdateAsync(product, ct);
        return RentalProductMapper.ToDto(product);
    }
}
