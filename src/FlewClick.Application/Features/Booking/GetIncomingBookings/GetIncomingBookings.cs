using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.GetIncomingBookings;

public record GetIncomingBookingsQuery(Guid ProfessionalProfileId) : IRequest<List<BookingDto>>;

public class GetIncomingBookingsValidator : AbstractValidator<GetIncomingBookingsQuery>
{
    public GetIncomingBookingsValidator()
    {
        RuleFor(x => x.ProfessionalProfileId).NotEmpty();
    }
}

public class GetIncomingBookingsHandler(IBookingRequestRepository bookingRepository)
    : IRequestHandler<GetIncomingBookingsQuery, List<BookingDto>>
{
    public async Task<List<BookingDto>> Handle(GetIncomingBookingsQuery request, CancellationToken ct)
    {
        var list = await bookingRepository.GetByProfessionalProfileIdAsync(request.ProfessionalProfileId, ct);
        return list.Select(BookingMapper.ToDto).ToList();
    }
}
