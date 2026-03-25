using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class SavedProfessional : Entity
{
    public Guid ConsumerId { get; private init; }
    public Guid ProfessionalProfileId { get; private init; }

    private SavedProfessional() { }

    public static SavedProfessional Create(Guid consumerId, Guid professionalProfileId)
    {
        if (consumerId == Guid.Empty)
            throw new DomainException("Consumer ID is required.");

        if (professionalProfileId == Guid.Empty)
            throw new DomainException("Professional profile ID is required.");

        return new SavedProfessional
        {
            ConsumerId = consumerId,
            ProfessionalProfileId = professionalProfileId
        };
    }
}
