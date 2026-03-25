using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Shortlist.SaveProfessional;

public record SaveProfessionalCommand(Guid ConsumerId, Guid ProfessionalProfileId) : IRequest;

public class SaveProfessionalValidator : AbstractValidator<SaveProfessionalCommand>
{
    public SaveProfessionalValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class SaveProfessionalHandler(
    IConsumerRepository consumerRepository,
    IProfessionalProfileRepository profileRepository,
    ISavedProfessionalRepository savedRepository)
    : IRequestHandler<SaveProfessionalCommand>
{
    public async Task Handle(SaveProfessionalCommand request, CancellationToken ct)
    {
        var consumer = await consumerRepository.GetByIdAsync(request.ConsumerId, ct)
            ?? throw new EntityNotFoundException("Consumer", request.ConsumerId);

        var profile = await profileRepository.GetByIdAsync(request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.ProfessionalProfileId);

        var existing = await savedRepository.GetByConsumerAndProfileAsync(
            request.ConsumerId, request.ProfessionalProfileId, ct);

        if (existing is not null)
            throw new DomainException("This professional is already in your shortlist.");

        var saved = SavedProfessional.Create(request.ConsumerId, request.ProfessionalProfileId);
        await savedRepository.AddAsync(saved, ct);
    }
}
