using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class ProfessionalProfile : Entity
{
    public Guid AppUserId { get; private init; }
    public string? Bio { get; private set; }
    public string? Location { get; private set; }
    public int? YearsOfExperience { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public string? PortfolioUrl { get; private set; }
    public bool IsRegistrationComplete { get; private set; }

    private ProfessionalProfile() { }

    public static ProfessionalProfile Create(
        Guid appUserId,
        string? bio,
        string? location,
        int? yearsOfExperience,
        decimal? hourlyRate,
        string? portfolioUrl)
    {
        if (appUserId == Guid.Empty)
            throw new DomainException("App user ID is required.");

        if (yearsOfExperience.HasValue && yearsOfExperience < 0)
            throw new DomainException("Years of experience cannot be negative.");

        if (hourlyRate.HasValue && hourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative.");

        return new ProfessionalProfile
        {
            AppUserId = appUserId,
            Bio = bio?.Trim(),
            Location = location?.Trim(),
            YearsOfExperience = yearsOfExperience,
            HourlyRate = hourlyRate,
            PortfolioUrl = portfolioUrl?.Trim(),
            IsRegistrationComplete = false
        };
    }

    public void UpdateBasicDetails(
        string? bio,
        string? location,
        int? yearsOfExperience,
        decimal? hourlyRate,
        string? portfolioUrl)
    {
        if (yearsOfExperience.HasValue && yearsOfExperience < 0)
            throw new DomainException("Years of experience cannot be negative.");

        if (hourlyRate.HasValue && hourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative.");

        Bio = bio?.Trim();
        Location = location?.Trim();
        YearsOfExperience = yearsOfExperience;
        HourlyRate = hourlyRate;
        PortfolioUrl = portfolioUrl?.Trim();
        Touch();
    }

    public void MarkRegistrationComplete()
    {
        IsRegistrationComplete = true;
        Touch();
    }
}
