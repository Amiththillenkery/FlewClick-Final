using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class RentalProduct : Entity
{
    public Guid RentalStoreId { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Category { get; private set; }
    public string? Brand { get; private set; }
    public string? Model { get; private set; }
    public ProductCondition Condition { get; private set; }
    public Dictionary<string, object?> Specifications { get; private set; } = new();
    public bool IsAvailable { get; private set; }

    private RentalProduct() { }

    public static RentalProduct Create(
        Guid rentalStoreId,
        string name,
        ProductCondition condition,
        string? description = null,
        string? category = null,
        string? brand = null,
        string? model = null,
        Dictionary<string, object?>? specifications = null)
    {
        if (rentalStoreId == Guid.Empty)
            throw new DomainException("Rental store ID is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        return new RentalProduct
        {
            RentalStoreId = rentalStoreId,
            Name = name.Trim(),
            Description = description?.Trim(),
            Category = category?.Trim(),
            Brand = brand?.Trim(),
            Model = model?.Trim(),
            Condition = condition,
            Specifications = specifications ?? new Dictionary<string, object?>(),
            IsAvailable = true
        };
    }

    public void Update(string name, string? description, string? category,
        string? brand, string? model, ProductCondition condition,
        Dictionary<string, object?>? specifications)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        Name = name.Trim();
        Description = description?.Trim();
        Category = category?.Trim();
        Brand = brand?.Trim();
        Model = model?.Trim();
        Condition = condition;
        if (specifications is not null)
            Specifications = specifications;
        Touch();
    }

    public void MarkAvailable() { IsAvailable = true; Touch(); }
    public void MarkUnavailable() { IsAvailable = false; Touch(); }
}
