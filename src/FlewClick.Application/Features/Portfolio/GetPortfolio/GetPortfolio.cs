using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.GetPortfolio;

public record GetPortfolioQuery(Guid ProfileId, bool VisibleOnly = true) : IRequest<List<PortfolioItemDto>>;

public class GetPortfolioValidator : AbstractValidator<GetPortfolioQuery>
{
    public GetPortfolioValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class GetPortfolioHandler(IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<GetPortfolioQuery, List<PortfolioItemDto>>
{
    public async Task<List<PortfolioItemDto>> Handle(GetPortfolioQuery request, CancellationToken ct)
    {
        var items = await portfolioItemRepository.GetByProfileIdAsync(
            request.ProfileId, request.VisibleOnly, ct);

        return items.Select(PortfolioMapper.ToDto).ToList();
    }
}
