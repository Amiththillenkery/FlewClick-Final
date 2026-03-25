using FlewClick.Application.Features.Booking.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Booking.GetBookingDetail;

public record GetBookingDetailQuery(Guid BookingId) : IRequest<BookingDetailDto>;

public class GetBookingDetailValidator : AbstractValidator<GetBookingDetailQuery>
{
    public GetBookingDetailValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}

public class GetBookingDetailHandler(
    IBookingRequestRepository bookingRepository,
    IConsumerRepository consumerRepository,
    IProfessionalProfileRepository profileRepository,
    IAppUserRepository userRepository,
    IPackageRepository packageRepository)
    : IRequestHandler<GetBookingDetailQuery, BookingDetailDto>
{
    public async Task<BookingDetailDto> Handle(GetBookingDetailQuery request, CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, ct)
            ?? throw new EntityNotFoundException("BookingRequest", request.BookingId);

        var consumer = await consumerRepository.GetByIdAsync(booking.ConsumerId, ct);
        var profile = await profileRepository.GetByIdAsync(booking.ProfessionalProfileId, ct);
        var package = await packageRepository.GetByIdAsync(booking.PackageId, ct);

        string? professionalName = null;
        if (profile is not null)
        {
            var user = await userRepository.GetByIdAsync(profile.AppUserId, ct);
            professionalName = user?.FullName;
        }

        return new BookingDetailDto(
            BookingMapper.ToDto(booking),
            consumer?.FullName,
            professionalName,
            package?.Name);
    }
}
