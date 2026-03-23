using FlewClick.Application.Features.RentalProducts.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalProducts.UploadProductImage;

public record UploadProductImageCommand(
    Guid ProductId,
    string ImageUrl,
    int DisplayOrder,
    bool IsPrimary
) : IRequest<RentalProductImageDto>;

public class UploadProductImageValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}

public class UploadProductImageHandler(
    IRentalProductRepository productRepository,
    IRentalProductImageRepository imageRepository)
    : IRequestHandler<UploadProductImageCommand, RentalProductImageDto>
{
    public async Task<RentalProductImageDto> Handle(UploadProductImageCommand request, CancellationToken ct)
    {
        _ = await productRepository.GetByIdAsync(request.ProductId, ct)
            ?? throw new EntityNotFoundException("RentalProduct", request.ProductId);

        var image = RentalProductImage.Create(
            request.ProductId, request.ImageUrl, request.DisplayOrder, request.IsPrimary);

        await imageRepository.AddAsync(image, ct);
        return RentalProductMapper.ToDto(image);
    }
}
