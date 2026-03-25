using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.GetConsumerBookings;

public record GetConsumerBookingsQuery(Guid ConsumerId) : IRequest<List<BookingDto>>;

public class GetConsumerBookingsValidator : AbstractValidator<GetConsumerBookingsQuery>
{
    public GetConsumerBookingsValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}

public class GetConsumerBookingsHandler(IBookingRequestRepository bookingRepository)
    : IRequestHandler<GetConsumerBookingsQuery, List<BookingDto>>
{
    public async Task<List<BookingDto>> Handle(GetConsumerBookingsQuery request, CancellationToken ct)
    {
        var list = await bookingRepository.GetByConsumerIdAsync(request.ConsumerId, ct);
        return list.Select(BookingMapper.ToDto).ToList();
    }
}
