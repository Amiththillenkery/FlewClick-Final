using FlewClick.Application.Interfaces;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.HandleDeauthorize;

public record HandleDeauthorizeCommand(string InstagramUserId) : IRequest;


public class HandleDeauthorizeHandler(
    IInstagramConnectionRepository connectionRepository)
    : IRequestHandler<HandleDeauthorizeCommand>
{
    public async Task Handle(HandleDeauthorizeCommand request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByInstagramUserIdAsync(request.InstagramUserId, ct);
        if (connection is null) return;

        connection.Deactivate();
        await connectionRepository.UpdateAsync(connection, ct);
    }
}
