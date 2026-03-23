using FlewClick.Application.Features.RentalStoreManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalStoreManagement.UpdateStoreConfig;

public record UpdateStoreConfigCommand(
    Guid StoreId,
    string StoreName,
    string? Description,
    Dictionary<string, object?>? Policies
) : IRequest<RentalStoreDto>;

public class UpdateStoreConfigValidator : AbstractValidator<UpdateStoreConfigCommand>
{
    public UpdateStoreConfigValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.StoreName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}

public class UpdateStoreConfigHandler(IRentalStoreRepository storeRepository)
    : IRequestHandler<UpdateStoreConfigCommand, RentalStoreDto>
{
    public async Task<RentalStoreDto> Handle(UpdateStoreConfigCommand request, CancellationToken ct)
    {
        var store = await storeRepository.GetByIdAsync(request.StoreId, ct)
            ?? throw new EntityNotFoundException("RentalStore", request.StoreId);

        store.Update(request.StoreName, request.Description, request.Policies);
        await storeRepository.UpdateAsync(store, ct);
        return RentalStoreMapper.ToDto(store);
    }
}
