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
    public ProfessionalRole? ProfessionalRole { get; private set; }
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
            ProfessionalRole = null,
            IsActive = true
        };
    }

    public static AppUser CreateProfessionalUser(
        string fullName,
        string email,
        ProfessionalRole professionalRole,
        string? phone = null)
    {
        ValidateBasicFields(fullName, email);

        return new AppUser
        {
            FullName = fullName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone?.Trim(),
            UserType = UserType.ProfessionalUser,
            ProfessionalRole = professionalRole,
            IsActive = true
        };
    }

    public void UpdateProfile(string fullName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        FullName = fullName.Trim();
        Phone = phone?.Trim();
        Touch();
    }

    public void ChangeProfessionalRole(ProfessionalRole role)
    {
        if (UserType != UserType.ProfessionalUser)
            throw new DomainException("Only professional users can have a professional role.");

        ProfessionalRole = role;
        Touch();
    }

    public void PromoteToProfessional(ProfessionalRole role)
    {
        UserType = UserType.ProfessionalUser;
        ProfessionalRole = role;
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
