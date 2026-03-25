using FlewClick.Application.Features.Portfolio.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.Portfolio.GetInstagramStatus;

public record GetInstagramStatusQuery(Guid ProfileId) : IRequest<InstagramConnectionDto?>;

public class GetInstagramStatusHandler(IInstagramConnectionRepository connectionRepository)
    : IRequestHandler<GetInstagramStatusQuery, InstagramConnectionDto?>
{
    public async Task<InstagramConnectionDto?> Handle(GetInstagramStatusQuery request, CancellationToken ct)
    {
        var connection = await connectionRepository.GetByProfileIdAsync(request.ProfileId, ct);
        return connection is null ? null : PortfolioMapper.ToDto(connection);
    }
}
