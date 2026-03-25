using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.DisconnectInstagram;

public record DisconnectInstagramCommand(Guid ProfileId, bool ClearPortfolio = false) : IRequest;

public class DisconnectInstagramValidator : AbstractValidator<DisconnectInstagramCommand>
{
    public DisconnectInstagramValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

public class DisconnectInstagramHandler(
    IInstagramConnectionRepository connectionRepository,
    IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<DisconnectInstagramCommand>
{
    public async Task Handle(DisconnectInstagramCommand request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("InstagramConnection for profile", request.ProfileId);

        connection.Deactivate();
        await connectionRepository.UpdateAsync(connection, ct);

        if (request.ClearPortfolio)
        {
            await portfolioItemRepository.RemoveByProfileIdAsync(request.ProfileId, ct);
        }
    }
}
