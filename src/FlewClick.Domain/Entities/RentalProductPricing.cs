using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RentalProductPricing : Entity
{
    public Guid RentalProductId { get; private init; }
    public decimal? HourlyRate { get; private set; }
    public decimal? DailyRate { get; private set; }
    public decimal? WeeklyRate { get; private set; }
    public decimal? MonthlyRate { get; private set; }
    public decimal DepositAmount { get; private set; }

    private RentalProductPricing() { }

    public static RentalProductPricing Create(
        Guid rentalProductId,
        decimal depositAmount,
        decimal? hourlyRate = null,
        decimal? dailyRate = null,
        decimal? weeklyRate = null,
        decimal? monthlyRate = null)
    {
        if (rentalProductId == Guid.Empty)
            throw new DomainException("Rental product ID is required.");

        ValidateRates(hourlyRate, dailyRate, weeklyRate, monthlyRate, depositAmount);

        return new RentalProductPricing
        {
            RentalProductId = rentalProductId,
            HourlyRate = hourlyRate,
            DailyRate = dailyRate,
            WeeklyRate = weeklyRate,
            MonthlyRate = monthlyRate,
            DepositAmount = depositAmount
        };
    }

    public void Update(decimal depositAmount, decimal? hourlyRate,
        decimal? dailyRate, decimal? weeklyRate, decimal? monthlyRate)
    {
        ValidateRates(hourlyRate, dailyRate, weeklyRate, monthlyRate, depositAmount);

        HourlyRate = hourlyRate;
        DailyRate = dailyRate;
        WeeklyRate = weeklyRate;
        MonthlyRate = monthlyRate;
        DepositAmount = depositAmount;
        Touch();
    }

    private static void ValidateRates(decimal? hourlyRate, decimal? dailyRate,
        decimal? weeklyRate, decimal? monthlyRate, decimal depositAmount)
    {
        if (hourlyRate is < 0) throw new DomainException("Hourly rate cannot be negative.");
        if (dailyRate is < 0) throw new DomainException("Daily rate cannot be negative.");
        if (weeklyRate is < 0) throw new DomainException("Weekly rate cannot be negative.");
        if (monthlyRate is < 0) throw new DomainException("Monthly rate cannot be negative.");
        if (depositAmount < 0) throw new DomainException("Deposit amount cannot be negative.");

        if (hourlyRate is null && dailyRate is null && weeklyRate is null && monthlyRate is null)
            throw new DomainException("At least one rental rate must be provided.");
    }
}
