using FlewClick.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Persistence;

public class FlewClickDbContext : DbContext
{
    public FlewClickDbContext(DbContextOptions<FlewClickDbContext> options) : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<ProfessionalProfile> ProfessionalProfiles => Set<ProfessionalProfile>();
    public DbSet<PhotographyConfig> PhotographyConfigs => Set<PhotographyConfig>();
    public DbSet<EditingConfig> EditingConfigs => Set<EditingConfig>();
    public DbSet<DroneConfig> DroneConfigs => Set<DroneConfig>();
    public DbSet<RentalEquipment> RentalEquipments => Set<RentalEquipment>();

    public DbSet<DeliverableMaster> DeliverableMasters => Set<DeliverableMaster>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageDeliverable> PackageDeliverables => Set<PackageDeliverable>();
    public DbSet<PackagePricing> PackagePricings => Set<PackagePricing>();
    public DbSet<RentalStore> RentalStores => Set<RentalStore>();
    public DbSet<RentalProduct> RentalProducts => Set<RentalProduct>();
    public DbSet<RentalProductImage> RentalProductImages => Set<RentalProductImage>();
    public DbSet<RentalProductPricing> RentalProductPricings => Set<RentalProductPricing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("app_users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(254).IsRequired();
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.UserType).HasColumnName("user_type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.ProfessionalRoles).HasColumnName("professional_roles").HasColumnType("jsonb");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.UserType);
        });

        modelBuilder.Entity<ProfessionalProfile>(entity =>
        {
            entity.ToTable("professional_profiles");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppUserId).HasColumnName("app_user_id").IsRequired();
            entity.Property(e => e.Bio).HasColumnName("bio").HasMaxLength(1000);
            entity.Property(e => e.Location).HasColumnName("location").HasMaxLength(200);
            entity.Property(e => e.YearsOfExperience).HasColumnName("years_of_experience");
            entity.Property(e => e.HourlyRate).HasColumnName("hourly_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.PortfolioUrl).HasColumnName("portfolio_url").HasMaxLength(500);
            entity.Property(e => e.IsRegistrationComplete).HasColumnName("is_registration_complete");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.AppUserId).IsUnique();
        });

        modelBuilder.Entity<PhotographyConfig>(entity =>
        {
            entity.ToTable("photography_configs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.Styles).HasColumnName("styles").HasColumnType("jsonb");
            entity.Property(e => e.CameraGear).HasColumnName("camera_gear").HasMaxLength(500);
            entity.Property(e => e.ShootTypes).HasColumnName("shoot_types").HasMaxLength(500);
            entity.Property(e => e.HasStudio).HasColumnName("has_studio");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId).IsUnique();
        });

        modelBuilder.Entity<EditingConfig>(entity =>
        {
            entity.ToTable("editing_configs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.SoftwareTools).HasColumnName("software_tools").HasColumnType("jsonb");
            entity.Property(e => e.Specializations).HasColumnName("specializations").HasColumnType("jsonb");
            entity.Property(e => e.OutputFormats).HasColumnName("output_formats").HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId).IsUnique();
        });

        modelBuilder.Entity<DroneConfig>(entity =>
        {
            entity.ToTable("drone_configs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.DroneModel).HasColumnName("drone_model").HasMaxLength(200).IsRequired();
            entity.Property(e => e.LicenseNumber).HasColumnName("license_number").HasMaxLength(100);
            entity.Property(e => e.HasFlightCertification).HasColumnName("has_flight_certification");
            entity.Property(e => e.MaxFlightAltitudeMeters).HasColumnName("max_flight_altitude_meters");
            entity.Property(e => e.Capabilities).HasColumnName("capabilities").HasColumnType("jsonb");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId).IsUnique();
        });

        modelBuilder.Entity<RentalEquipment>(entity =>
        {
            entity.ToTable("rental_equipments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.EquipmentType).HasColumnName("equipment_type").HasMaxLength(100);
            entity.Property(e => e.Brand).HasColumnName("brand").HasMaxLength(100);
            entity.Property(e => e.DailyRentalRate).HasColumnName("daily_rental_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.ConditionNotes).HasColumnName("condition_notes").HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId);
        });

        modelBuilder.Entity<DeliverableMaster>(entity =>
        {
            entity.ToTable("deliverable_masters");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleType).HasColumnName("role_type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
            entity.Property(e => e.Category).HasColumnName("category").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.ConfigurableAttributes).HasColumnName("configurable_attrs").HasColumnType("jsonb");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.RoleType);
            entity.HasIndex(e => new { e.RoleType, e.Category });
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("packages");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(2000);
            entity.Property(e => e.PackageType).HasColumnName("package_type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.CoverageType).HasColumnName("coverage_type").HasConversion<string?>().HasMaxLength(30);
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId);
            entity.HasIndex(e => new { e.ProfessionalProfileId, e.PackageType });
        });

        modelBuilder.Entity<PackageDeliverable>(entity =>
        {
            entity.ToTable("package_deliverables");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id").IsRequired();
            entity.Property(e => e.DeliverableMasterId).HasColumnName("deliverable_master_id").IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Configuration).HasColumnName("configuration").HasColumnType("jsonb");
            entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.PackageId);
            entity.HasIndex(e => new { e.PackageId, e.DeliverableMasterId }).IsUnique();
        });

        modelBuilder.Entity<PackagePricing>(entity =>
        {
            entity.ToTable("package_pricings");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id").IsRequired();
            entity.Property(e => e.PricingType).HasColumnName("pricing_type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.BasePrice).HasColumnName("base_price").HasColumnType("decimal(12,2)");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage").HasColumnType("decimal(5,2)");
            entity.Property(e => e.FinalPrice).HasColumnName("final_price").HasColumnType("decimal(12,2)");
            entity.Property(e => e.DurationHours).HasColumnName("duration_hours");
            entity.Property(e => e.IsNegotiable).HasColumnName("is_negotiable");
            entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.PackageId).IsUnique();
        });

        modelBuilder.Entity<RentalStore>(entity =>
        {
            entity.ToTable("rental_stores");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.StoreName).HasColumnName("store_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(2000);
            entity.Property(e => e.Policies).HasColumnName("policies").HasColumnType("jsonb");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId).IsUnique();
        });

        modelBuilder.Entity<RentalProduct>(entity =>
        {
            entity.ToTable("rental_products");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RentalStoreId).HasColumnName("rental_store_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(2000);
            entity.Property(e => e.Category).HasColumnName("category").HasMaxLength(100);
            entity.Property(e => e.Brand).HasColumnName("brand").HasMaxLength(100);
            entity.Property(e => e.Model).HasColumnName("model").HasMaxLength(200);
            entity.Property(e => e.Condition).HasColumnName("condition").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.Specifications).HasColumnName("specifications").HasColumnType("jsonb");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.RentalStoreId);
            entity.HasIndex(e => new { e.RentalStoreId, e.Category });
        });

        modelBuilder.Entity<RentalProductImage>(entity =>
        {
            entity.ToTable("rental_product_images");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RentalProductId).HasColumnName("rental_product_id").IsRequired();
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasMaxLength(1000).IsRequired();
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.RentalProductId);
        });

        modelBuilder.Entity<RentalProductPricing>(entity =>
        {
            entity.ToTable("rental_product_pricings");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RentalProductId).HasColumnName("rental_product_id").IsRequired();
            entity.Property(e => e.HourlyRate).HasColumnName("hourly_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.DailyRate).HasColumnName("daily_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.WeeklyRate).HasColumnName("weekly_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.MonthlyRate).HasColumnName("monthly_rate").HasColumnType("decimal(10,2)");
            entity.Property(e => e.DepositAmount).HasColumnName("deposit_amount").HasColumnType("decimal(10,2)");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.RentalProductId).IsUnique();
        });
    }
}
