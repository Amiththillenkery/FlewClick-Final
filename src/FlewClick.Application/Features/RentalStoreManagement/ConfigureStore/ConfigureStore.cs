using FlewClick.Application.Features.RentalStoreManagement.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.RentalStoreManagement.ConfigureStore;

public record ConfigureStoreCommand(
    Guid ProfileId,
    string StoreName,
    string? Description,
    Dictionary<string, object?>? Policies
) : IRequest<RentalStoreDto>;

public class ConfigureStoreValidator : AbstractValidator<ConfigureStoreCommand>
{
    public ConfigureStoreValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.StoreName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}

public class ConfigureStoreHandler(
    IRentalStoreRepository storeRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<ConfigureStoreCommand, RentalStoreDto>
{
    public async Task<RentalStoreDto> Handle(ConfigureStoreCommand request, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfileId);

        var user = await userRepository.GetByIdAsync(profile.AppUserId, ct)
            ?? throw new EntityNotFoundException("AppUser", profile.AppUserId);

        if (!user.HasRole(ProfessionalRole.DigitalRental))
            throw new DomainException("Only Digital Rental professionals can configure a rental store.");

        var existing = await storeRepository.GetByProfileIdAsync(request.ProfileId, ct);
        if (existing is not null)
            throw new DomainException("A rental store already exists for this profile. Use update instead.");

        var store = RentalStore.Create(
            request.ProfileId, request.StoreName, request.Description, request.Policies);

        await storeRepository.AddAsync(store, ct);
        return RentalStoreMapper.ToDto(store);
    }
}
