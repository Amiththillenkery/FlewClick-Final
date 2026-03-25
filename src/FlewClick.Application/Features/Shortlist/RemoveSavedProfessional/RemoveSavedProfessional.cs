using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Shortlist.RemoveSavedProfessional;

public record RemoveSavedProfessionalCommand(Guid ConsumerId, Guid ProfessionalProfileId) : IRequest;

public class RemoveSavedProfessionalValidator : AbstractValidator<RemoveSavedProfessionalCommand>
{
    public RemoveSavedProfessionalValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class RemoveSavedProfessionalHandler(ISavedProfessionalRepository savedRepository)
    : IRequestHandler<RemoveSavedProfessionalCommand>
{
    public async Task Handle(RemoveSavedProfessionalCommand request, CancellationToken ct)
    {
        var saved = await savedRepository.GetByConsumerAndProfileAsync(
            request.ConsumerId, request.ProfessionalProfileId, ct)
            ?? throw new EntityNotFoundException("SavedProfessional",
                $"Consumer {request.ConsumerId} / Profile {request.ProfessionalProfileId}");

        await savedRepository.RemoveAsync(saved, ct);
    }
}
