using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.ProfileSettings.UpdateOtherProfileOptions;

public record UpdateOtherProfileOptionsCommand(
    Guid UserId,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl
) : IRequest;

public class UpdateOtherProfileOptionsValidator : AbstractValidator<UpdateOtherProfileOptionsCommand>
{
    public UpdateOtherProfileOptionsValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Bio).MaximumLength(1000).When(x => x.Bio is not null);
        RuleFor(x => x.Location).MaximumLength(200).When(x => x.Location is not null);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).When(x => x.YearsOfExperience.HasValue);
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0).When(x => x.HourlyRate.HasValue);
        RuleFor(x => x.PortfolioUrl).MaximumLength(500).When(x => x.PortfolioUrl is not null);
    }
}

public class UpdateOtherProfileOptionsHandler(IProfessionalProfileRepository repository)
    : IRequestHandler<UpdateOtherProfileOptionsCommand>
{
    public async Task Handle(UpdateOtherProfileOptionsCommand request, CancellationToken ct)
    {
        var profile = await repository.GetByAppUserIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException("ProfessionalProfile", request.UserId);

        profile.UpdateBasicDetails(
            request.Bio,
            request.Location,
            request.YearsOfExperience,
            request.HourlyRate,
            request.PortfolioUrl);
            
        await repository.UpdateAsync(profile, ct);
    }
}
