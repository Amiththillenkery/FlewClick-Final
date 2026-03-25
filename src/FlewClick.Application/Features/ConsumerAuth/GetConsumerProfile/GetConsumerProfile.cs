using FlewClick.Application.Features.ConsumerAuth.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.ConsumerAuth.GetConsumerProfile;

public record GetConsumerProfileQuery(Guid ConsumerId) : IRequest<ConsumerDto>;

public class GetConsumerProfileHandler(IConsumerRepository consumerRepository)
    : IRequestHandler<GetConsumerProfileQuery, ConsumerDto>
{
    public async Task<ConsumerDto> Handle(GetConsumerProfileQuery request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByIdAsync(request.ConsumerId, ct)
            ?? throw new EntityNotFoundException("Consumer", request.ConsumerId);

        return ConsumerMapper.ToDto(consumer);
    }
}
