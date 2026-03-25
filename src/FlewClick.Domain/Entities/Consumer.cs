using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class Consumer : Entity
{
    public string Phone { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public bool IsPhoneVerified { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private Consumer() { }

    public static Consumer Create(string phone, string fullName)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name is required.");

        return new Consumer
        {
            Phone = phone.Trim(),
            FullName = fullName.Trim(),
            IsPhoneVerified = false,
            IsActive = true
        };
    }

    public void VerifyPhone()
    {
        IsPhoneVerified = true;
        Touch();
    }

    public void UpdateProfile(string fullName, string? email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name is required.");

        FullName = fullName.Trim();
        Email = email?.Trim();
        Touch();
    }

    public void MarkLoggedIn()
    {
        LastLoginAt = DateTime.UtcNow;
        Touch();
    }

    public void Activate() { IsActive = true; Touch(); }
    public void Deactivate() { IsActive = false; Touch(); }
}
