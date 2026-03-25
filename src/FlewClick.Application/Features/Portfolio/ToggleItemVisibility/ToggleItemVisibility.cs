using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.ToggleItemVisibility;

public record ToggleItemVisibilityCommand(Guid ItemId) : IRequest<PortfolioItemDto>;

public class ToggleItemVisibilityValidator : AbstractValidator<ToggleItemVisibilityCommand>
{
    public ToggleItemVisibilityValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
    }
}

public class ToggleItemVisibilityHandler(IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<ToggleItemVisibilityCommand, PortfolioItemDto>
{
    public async Task<PortfolioItemDto> Handle(ToggleItemVisibilityCommand request, CancellationToken ct)
    {
        var item = await portfolioItemRepository.GetByIdAsync(request.ItemId, ct)
            ?? throw new EntityNotFoundException("PortfolioItem", request.ItemId);

        item.SetVisibility(!item.IsVisible);
        await portfolioItemRepository.UpdateAsync(item, ct);

        return PortfolioMapper.ToDto(item);
    }
}
