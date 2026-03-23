using FlewClick.Domain.Common;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RentalProductImage : Entity
{
    public Guid RentalProductId { get; private init; }
    public string ImageUrl { get; private set; } = string.Empty;
    public int DisplayOrder { get; private set; }
    public bool IsPrimary { get; private set; }

    private RentalProductImage() { }

    public static RentalProductImage Create(
        Guid rentalProductId,
        string imageUrl,
        int displayOrder,
        bool isPrimary = false)
    {
        if (rentalProductId == Guid.Empty)
            throw new DomainException("Rental product ID is required.");

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("Image URL is required.");

        if (displayOrder < 0)
            throw new DomainException("Display order cannot be negative.");

        return new RentalProductImage
        {
            RentalProductId = rentalProductId,
            ImageUrl = imageUrl.Trim(),
            DisplayOrder = displayOrder,
            IsPrimary = isPrimary
        };
    }

    public void SetAsPrimary() { IsPrimary = true; Touch(); }
    public void UnsetPrimary() { IsPrimary = false; Touch(); }
    public void UpdateOrder(int order)
    {
        if (order < 0) throw new DomainException("Display order cannot be negative.");
        DisplayOrder = order;
        Touch();
    }
}
