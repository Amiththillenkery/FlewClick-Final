using FlewClick.Application.Features.RentalStoreManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using MediatR;

namespace FlewClick.Application.Features.RentalStoreManagement.GetStoreByProfile;

public record GetStoreByProfileQuery(Guid ProfileId) : IRequest<RentalStoreDto>;

public class GetStoreByProfileHandler(IRentalStoreRepository storeRepository)
    : IRequestHandler<GetStoreByProfileQuery, RentalStoreDto>
{
    public async Task<RentalStoreDto> Handle(GetStoreByProfileQuery request, CancellationToken ct)
    {
        var store = await storeRepository.GetByProfileIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("RentalStore for profile", request.ProfileId);

        return RentalStoreMapper.ToDto(store);
    }
}
