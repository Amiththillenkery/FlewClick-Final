using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class AppUser : Entity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public UserType UserType { get; private set; }
    public List<ProfessionalRole> ProfessionalRoles { get; private set; } = [];
    public bool IsActive { get; private set; }

    private AppUser() { }

    public static AppUser CreateUser(string fullName, string email, string? phone = null)
    {
        ValidateBasicFields(fullName, email);

        return new AppUser
        {
            FullName = fullName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone?.Trim(),
            UserType = UserType.User,
            ProfessionalRoles = [],
            IsActive = true
        };
    }

    public static AppUser CreateProfessionalUser(
        string fullName,
        string email,
        List<ProfessionalRole> professionalRoles,
        string? phone = null)
    {
        ValidateBasicFields(fullName, email);

        if (professionalRoles.Count == 0)
            throw new DomainException("At least one professional role is required.");

        return new AppUser
        {
            FullName = fullName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone?.Trim(),
            UserType = UserType.ProfessionalUser,
            ProfessionalRoles = professionalRoles.Distinct().ToList(),
            IsActive = true
        };
    }

    public bool HasRole(ProfessionalRole role) => ProfessionalRoles.Contains(role);

    public bool HasAnyRole(params ProfessionalRole[] roles) => roles.Any(r => ProfessionalRoles.Contains(r));

    public void UpdateProfile(string fullName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        FullName = fullName.Trim();
        Phone = phone?.Trim();
        Touch();
    }

    public void AddProfessionalRole(ProfessionalRole role)
    {
        if (UserType != UserType.ProfessionalUser)
            throw new DomainException("Only professional users can have professional roles.");

        if (!ProfessionalRoles.Contains(role))
        {
            ProfessionalRoles.Add(role);
            Touch();
        }
    }

    public void RemoveProfessionalRole(ProfessionalRole role)
    {
        if (ProfessionalRoles.Count <= 1)
            throw new DomainException("A professional user must have at least one role.");

        ProfessionalRoles.Remove(role);
        Touch();
    }

    public void PromoteToProfessional(List<ProfessionalRole> roles)
    {
        if (roles.Count == 0)
            throw new DomainException("At least one professional role is required.");

        UserType = UserType.ProfessionalUser;
        ProfessionalRoles = roles.Distinct().ToList();
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    private static void ValidateBasicFields(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

        if (!email.Contains('@'))
            throw new DomainException("Email format is invalid.");
    }
}
