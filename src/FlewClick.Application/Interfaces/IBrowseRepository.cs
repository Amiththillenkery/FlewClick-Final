using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public record ProfessionalListingItem(
    Guid ProfileId,
    Guid AppUserId,
    string FullName,
    string? Bio,
    string? Location,
    int? YearsOfExperience,
    decimal? HourlyRate,
    string? PortfolioUrl,
    List<ProfessionalRole> ProfessionalRoles,
    DateTime CreatedAtUtc
);

public record CategoryCount(ProfessionalRole Role, int Count);

public record PaginatedResult<T>(List<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public interface IBrowseRepository
{
    Task<List<CategoryCount>> GetCategoryCountsAsync(CancellationToken ct = default);

    Task<PaginatedResult<ProfessionalListingItem>> BrowseProfessionalsAsync(
        ProfessionalRole? role = null,
        string? location = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    Task<PaginatedResult<ProfessionalListingItem>> SearchProfessionalsAsync(
        string query,
        ProfessionalRole? role = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);
}
