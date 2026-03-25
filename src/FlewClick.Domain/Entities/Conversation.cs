using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class Conversation : Entity
{
    public Guid BookingRequestId { get; private init; }
    public Guid ConsumerId { get; private init; }
    public Guid ProfessionalProfileId { get; private init; }
    public bool IsActive { get; private set; }

    private Conversation() { }

    public static Conversation Create(Guid bookingRequestId, Guid consumerId, Guid professionalProfileId)
    {
        if (bookingRequestId == Guid.Empty) throw new DomainException("Booking request ID is required.");
        if (consumerId == Guid.Empty) throw new DomainException("Consumer ID is required.");
        if (professionalProfileId == Guid.Empty) throw new DomainException("Professional profile ID is required.");

        return new Conversation
        {
            BookingRequestId = bookingRequestId,
            ConsumerId = consumerId,
            ProfessionalProfileId = professionalProfileId,
            IsActive = true
        };
    }

    public void Deactivate() { IsActive = false; Touch(); }
    public void Activate() { IsActive = true; Touch(); }
}
