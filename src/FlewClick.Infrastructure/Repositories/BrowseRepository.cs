using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class BrowseRepository(FlewClickDbContext context) : IBrowseRepository
{
    public async Task<List<CategoryCount>> GetCategoryCountsAsync(CancellationToken ct = default)
    {
        var professionals = await context.AppUsers
            .Where(u => u.UserType == UserType.ProfessionalUser && u.IsActive)
            .Select(u => u.ProfessionalRoles)
            .ToListAsync(ct);

        var roleCounts = professionals
            .SelectMany(roles => roles)
            .GroupBy(r => r)
            .Select(g => new CategoryCount(g.Key, g.Count()))
            .OrderBy(c => c.Role)
            .ToList();

        return roleCounts;
    }

    public async Task<PaginatedResult<ProfessionalListingItem>> BrowseProfessionalsAsync(
        ProfessionalRole? role = null,
        string? location = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = from profile in context.ProfessionalProfiles
                    join user in context.AppUsers on profile.AppUserId equals user.Id
                    where user.IsActive
                          && user.UserType == UserType.ProfessionalUser
                          && profile.IsRegistrationComplete
                    select new { profile, user };

        if (role.HasValue)
            query = query.Where(x => x.user.ProfessionalRoles.Contains(role.Value));

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(x => x.profile.Location != null
                && EF.Functions.ILike(x.profile.Location, $"%{location}%"));

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.profile.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProfessionalListingItem(
                x.profile.Id,
                x.user.Id,
                x.user.FullName,
                x.profile.Bio,
                x.profile.Location,
                x.profile.YearsOfExperience,
                x.profile.HourlyRate,
                x.profile.PortfolioUrl,
                x.user.ProfessionalRoles,
                x.profile.CreatedAtUtc
            ))
            .ToListAsync(ct);

        return new PaginatedResult<ProfessionalListingItem>(items, totalCount, page, pageSize);
    }

    public async Task<PaginatedResult<ProfessionalListingItem>> SearchProfessionalsAsync(
        string searchQuery,
        ProfessionalRole? role = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = from profile in context.ProfessionalProfiles
                    join user in context.AppUsers on profile.AppUserId equals user.Id
                    where user.IsActive
                          && user.UserType == UserType.ProfessionalUser
                          && profile.IsRegistrationComplete
                    select new { profile, user };

        query = query.Where(x =>
            EF.Functions.ILike(x.user.FullName, $"%{searchQuery}%")
            || (x.profile.Bio != null && EF.Functions.ILike(x.profile.Bio, $"%{searchQuery}%"))
            || (x.profile.Location != null && EF.Functions.ILike(x.profile.Location, $"%{searchQuery}%"))
        );

        if (role.HasValue)
            query = query.Where(x => x.user.ProfessionalRoles.Contains(role.Value));

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.profile.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProfessionalListingItem(
                x.profile.Id,
                x.user.Id,
                x.user.FullName,
                x.profile.Bio,
                x.profile.Location,
                x.profile.YearsOfExperience,
                x.profile.HourlyRate,
                x.profile.PortfolioUrl,
                x.user.ProfessionalRoles,
                x.profile.CreatedAtUtc
            ))
            .ToListAsync(ct);

        return new PaginatedResult<ProfessionalListingItem>(items, totalCount, page, pageSize);
    }
}
