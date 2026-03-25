using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.HandleDataDeletion;

public record HandleDataDeletionCommand(string InstagramUserId) : IRequest<HandleDataDeletionResponse>;

public record HandleDataDeletionResponse(string Url, string ConfirmationCode);

public class HandleDataDeletionHandler(
    IInstagramConnectionRepository connectionRepository,
    IPortfolioItemRepository portfolioItemRepository)
    : IRequestHandler<HandleDataDeletionCommand, HandleDataDeletionResponse>
{
    public async Task<HandleDataDeletionResponse> Handle(HandleDataDeletionCommand request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByInstagramUserIdAsync(request.InstagramUserId, ct);

        if (connection is not null)
        {
            await portfolioItemRepository.RemoveByProfileIdAsync(connection.ProfessionalProfileId, ct);
            await connectionRepository.RemoveAsync(connection, ct);
        }

        var confirmationCode = Guid.NewGuid().ToString("N")[..12];
        return new HandleDataDeletionResponse(
            $"/api/portfolio/instagram/data-deletion/status?code={confirmationCode}",
            confirmationCode);
    }
}
