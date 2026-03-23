using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.AddProduct;

public record AddProductCommand(
    Guid StoreId,
    string Name,
    ProductCondition Condition,
    string? Description,
    string? Category,
    string? Brand,
    string? Model,
    Dictionary<string, object?>? Specifications
) : IRequest<RentalProductDto>;

public class AddProductValidator : AbstractValidator<AddProductCommand>
{
    public AddProductValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Condition).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.Category).MaximumLength(100).When(x => x.Category is not null);
        RuleFor(x => x.Brand).MaximumLength(100).When(x => x.Brand is not null);
        RuleFor(x => x.Model).MaximumLength(200).When(x => x.Model is not null);
    }
}

public class AddProductHandler(
    IRentalStoreRepository storeRepository,
    IRentalProductRepository productRepository)
    : IRequestHandler<AddProductCommand, RentalProductDto>
{
    public async Task<RentalProductDto> Handle(AddProductCommand request, CancellationToken ct)
    {
        _ = await storeRepository.GetByIdAsync(request.StoreId, ct)
            ?? throw new EntityNotFoundException("RentalStore", request.StoreId);

        var product = RentalProduct.Create(
            request.StoreId, request.Name, request.Condition,
            request.Description, request.Category, request.Brand,
            request.Model, request.Specifications);

        await productRepository.AddAsync(product, ct);
        return RentalProductMapper.ToDto(product);
    }
}
