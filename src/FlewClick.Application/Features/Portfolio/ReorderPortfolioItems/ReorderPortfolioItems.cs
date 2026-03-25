using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.ReorderPortfolioItems;

public record ReorderPortfolioItemsCommand(Guid ProfileId, List<Guid> OrderedItemIds) : IRequest;

public class ReorderPortfolioItemsValidator : AbstractValidator<ReorderPortfolioItemsCommand>
{
    public ReorderPortfolioItemsValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.OrderedItemIds).NotEmpty().WithMessage("Item ID list cannot be empty.");
    }
}

public class ReorderPortfolioItemsHandler(IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<ReorderPortfolioItemsCommand>
{
    public async Task Handle(ReorderPortfolioItemsCommand request, CancellationToken ct)
    {
        var items = await portfolioItemRepository.GetByProfileIdAsync(request.ProfileId, ct: ct);

        if (items.Count == 0)
            throw new DomainException("No portfolio items found for this profile.");

        var itemsById = items.ToDictionary(i => i.Id);
        var updated = new List<Domain.Entities.PortfolioItem>();

        for (int i = 0; i < request.OrderedItemIds.Count; i++)
        {
            if (itemsById.TryGetValue(request.OrderedItemIds[i], out var item))
            {
                item.SetDisplayOrder(i);
                updated.Add(item);
            }
        }

        if (updated.Count > 0)
            await portfolioItemRepository.UpdateRangeAsync(updated, ct);
    }
}
