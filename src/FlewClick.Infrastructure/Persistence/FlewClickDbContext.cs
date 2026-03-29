using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Persistence;

public class FlewClickDbContext : DbContext
{
    public FlewClickDbContext(DbContextOptions<FlewClickDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

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

    public DbSet<InstagramConnection> InstagramConnections => Set<InstagramConnection>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();

    public DbSet<Consumer> Consumers => Set<Consumer>();
    public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();
    public DbSet<SavedProfessional> SavedProfessionals => Set<SavedProfessional>();

    public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();
    public DbSet<Agreement> Agreements => Set<Agreement>();
    public DbSet<AgreementDeliverable> AgreementDeliverables => Set<AgreementDeliverable>();
    public DbSet<PlatformFeePayment> PlatformFeePayments => Set<PlatformFeePayment>();
    public DbSet<BookingStatusHistory> BookingStatusHistory => Set<BookingStatusHistory>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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

            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(500);

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

            var seed = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var P = ProfessionalRole.Photographer;
            var V = ProfessionalRole.Videographer;
            var E = ProfessionalRole.Editor;
            var D = ProfessionalRole.DroneOwner;

            entity.HasData(
                // ── Photographer deliverables ──
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000001"), RoleType = P, Name = "Photo Album", Description = (string?)"Printed photo album with customizable pages", Category = DeliverableCategory.Print, ConfigurableAttributes = new Dictionary<string, object?> { ["leaf_count"] = 40, ["size"] = "A4" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000002"), RoleType = P, Name = "Canvas Print", Description = (string?)"Gallery-wrapped canvas photo print", Category = DeliverableCategory.Print, ConfigurableAttributes = new Dictionary<string, object?> { ["size"] = "16x20", ["frame"] = "black" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000003"), RoleType = P, Name = "Framed Photo", Description = (string?)"Single framed photograph with mat", Category = DeliverableCategory.Print, ConfigurableAttributes = new Dictionary<string, object?> { ["size"] = "8x10", ["frame_material"] = "wood" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000004"), RoleType = P, Name = "Edited Digital Photos", Description = (string?)"Professionally edited high-resolution digital photos", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["resolution"] = "full", ["format"] = "JPEG" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000005"), RoleType = P, Name = "Raw Unedited Photos", Description = (string?)"All raw unedited photos from the shoot", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["format"] = "RAW+JPEG" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000006"), RoleType = P, Name = "Online Gallery", Description = (string?)"Password-protected online gallery for sharing", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["duration_days"] = 90 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000007"), RoleType = P, Name = "USB Drive", Description = (string?)"All photos delivered on a branded USB drive", Category = DeliverableCategory.Physical, ConfigurableAttributes = new Dictionary<string, object?> { ["capacity_gb"] = 32 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000001-0000-0000-0000-000000000008"), RoleType = P, Name = "Photo Booth Setup", Description = (string?)"On-site photo booth with props and instant prints", Category = DeliverableCategory.Service, ConfigurableAttributes = new Dictionary<string, object?> { ["duration_hours"] = 3, ["props_included"] = true }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },

                // ── Videographer deliverables ──
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000001"), RoleType = V, Name = "Highlight Reel", Description = (string?)"Short cinematic highlight video of the event", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["duration_minutes"] = 5, ["resolution"] = "4K" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000002"), RoleType = V, Name = "Full Event Video", Description = (string?)"Complete uncut video coverage of the event", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["resolution"] = "4K" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000003"), RoleType = V, Name = "Teaser Video", Description = (string?)"30-60 second social media teaser clip", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["duration_seconds"] = 60, ["aspect_ratio"] = "9:16" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000004"), RoleType = V, Name = "Drone Aerial Footage", Description = (string?)"Aerial video footage captured by drone", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["resolution"] = "4K" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000005"), RoleType = V, Name = "Same Day Edit", Description = (string?)"Quick-turnaround edited video delivered same day", Category = DeliverableCategory.Service, ConfigurableAttributes = new Dictionary<string, object?> { ["duration_minutes"] = 3 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000006"), RoleType = V, Name = "Raw Video Footage", Description = (string?)"All raw unedited video clips from the shoot", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["format"] = "MP4" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000002-0000-0000-0000-000000000007"), RoleType = V, Name = "DVD/Blu-ray", Description = (string?)"Finished video on physical disc with custom label", Category = DeliverableCategory.Physical, ConfigurableAttributes = new Dictionary<string, object?> { ["format"] = "Blu-ray", ["copies"] = 2 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },

                // ── Editor deliverables ──
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000001"), RoleType = E, Name = "Photo Retouching", Description = (string?)"Professional retouching per photo (skin, color, exposure)", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["level"] = "advanced" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000002"), RoleType = E, Name = "Color Grading", Description = (string?)"Cinematic color grading for video footage", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["style"] = "cinematic" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000003"), RoleType = E, Name = "Video Editing", Description = (string?)"Full video editing with transitions, music, and titles", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["revisions"] = 2 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000004"), RoleType = E, Name = "Photo Compositing", Description = (string?)"Composite multiple photos into artistic collage", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["photos_count"] = 5 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000005"), RoleType = E, Name = "Album Design", Description = (string?)"Custom album layout design with professional typography", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["pages"] = 40, ["style"] = "magazine" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000003-0000-0000-0000-000000000006"), RoleType = E, Name = "Background Removal", Description = (string?)"Professional background removal and replacement", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?>(), IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },

                // ── Drone Owner deliverables ──
                new { Id = Guid.Parse("a0000004-0000-0000-0000-000000000001"), RoleType = D, Name = "Aerial Photography", Description = (string?)"High-resolution aerial still photographs", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["resolution"] = "48MP" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000004-0000-0000-0000-000000000002"), RoleType = D, Name = "Aerial Video", Description = (string?)"Cinematic aerial video footage", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["resolution"] = "4K", ["duration_minutes"] = 10 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000004-0000-0000-0000-000000000003"), RoleType = D, Name = "360 Panorama", Description = (string?)"Aerial 360-degree panoramic photo", Category = DeliverableCategory.Digital, ConfigurableAttributes = new Dictionary<string, object?> { ["format"] = "equirectangular" }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000004-0000-0000-0000-000000000004"), RoleType = D, Name = "Property Survey", Description = (string?)"Aerial mapping and survey flight for property", Category = DeliverableCategory.Service, ConfigurableAttributes = new Dictionary<string, object?> { ["area_acres"] = 5 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null },
                new { Id = Guid.Parse("a0000004-0000-0000-0000-000000000005"), RoleType = D, Name = "Live Streaming", Description = (string?)"Real-time aerial live streaming of the event", Category = DeliverableCategory.Service, ConfigurableAttributes = new Dictionary<string, object?> { ["platform"] = "YouTube", ["duration_hours"] = 2 }, IsActive = true, CreatedAtUtc = seed, UpdatedAtUtc = (DateTime?)null }
            );
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

            entity.HasMany(e => e.Deliverables)
                .WithOne()
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Pricing)
                .WithOne()
                .HasForeignKey<PackagePricing>(p => p.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<InstagramConnection>(entity =>
        {
            entity.ToTable("instagram_connections");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.InstagramUserId).HasColumnName("instagram_user_id").HasMaxLength(100).IsRequired();
            entity.Property(e => e.AccessToken).HasColumnName("access_token").HasMaxLength(1000).IsRequired();
            entity.Property(e => e.TokenExpiresAt).HasColumnName("token_expires_at").IsRequired();
            entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastSyncAt).HasColumnName("last_sync_at");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId).IsUnique();
            entity.HasIndex(e => e.InstagramUserId);
        });

        modelBuilder.Entity<PortfolioItem>(entity =>
        {
            entity.ToTable("portfolio_items");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.InstagramMediaId).HasColumnName("instagram_media_id").HasMaxLength(100).IsRequired();
            entity.Property(e => e.MediaType).HasColumnName("media_type").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.MediaUrl).HasColumnName("media_url").HasMaxLength(2000).IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").HasMaxLength(2000);
            entity.Property(e => e.Caption).HasColumnName("caption").HasMaxLength(2200);
            entity.Property(e => e.Permalink).HasColumnName("permalink").HasMaxLength(500).IsRequired();
            entity.Property(e => e.PostedAt).HasColumnName("posted_at").IsRequired();
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ProfessionalProfileId);
            entity.HasIndex(e => new { e.ProfessionalProfileId, e.InstagramMediaId }).IsUnique();
        });

        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.ToTable("consumers");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
            entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(254);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(500);
            entity.Property(e => e.IsPhoneVerified).HasColumnName("is_phone_verified");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.Phone).IsUnique();
        });

        modelBuilder.Entity<OtpVerification>(entity =>
        {
            entity.ToTable("otp_verifications");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(6).IsRequired();
            entity.Property(e => e.Purpose).HasColumnName("purpose").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at").IsRequired();
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => new { e.Phone, e.Purpose });
        });

        modelBuilder.Entity<SavedProfessional>(entity =>
        {
            entity.ToTable("saved_professionals");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsumerId).HasColumnName("consumer_id").IsRequired();
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => new { e.ConsumerId, e.ProfessionalProfileId }).IsUnique();
            entity.HasIndex(e => e.ConsumerId);
        });

        modelBuilder.Entity<BookingRequest>(entity =>
        {
            entity.ToTable("booking_requests");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsumerId).HasColumnName("consumer_id").IsRequired();
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.PackageId).HasColumnName("package_id").IsRequired();
            entity.Property(e => e.EventDate).HasColumnName("event_date").IsRequired();
            entity.Property(e => e.EventLocation).HasColumnName("event_location").HasMaxLength(500);
            entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(2000);
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.DeclineReason).HasColumnName("decline_reason").HasMaxLength(1000);
            entity.Property(e => e.CancellationReason).HasColumnName("cancellation_reason").HasMaxLength(1000);
            entity.Property(e => e.CancelledBy).HasColumnName("cancelled_by").HasConversion<string?>().HasMaxLength(20);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.ConsumerId);
            entity.HasIndex(e => e.ProfessionalProfileId);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<Agreement>(entity =>
        {
            entity.ToTable("agreements");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingRequestId).HasColumnName("booking_request_id").IsRequired();
            entity.Property(e => e.Version).HasColumnName("version").IsRequired();
            entity.Property(e => e.PackageSnapshot).HasColumnName("package_snapshot").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.EventDate).HasColumnName("event_date").IsRequired();
            entity.Property(e => e.EventLocation).HasColumnName("event_location").HasMaxLength(500);
            entity.Property(e => e.EventDescription).HasColumnName("event_description").HasMaxLength(2000);
            entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Terms).HasColumnName("terms").HasMaxLength(5000);
            entity.Property(e => e.Conditions).HasColumnName("conditions").HasMaxLength(5000);
            entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(2000);
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.BookingRequestId);
            entity.HasIndex(e => new { e.BookingRequestId, e.Version }).IsUnique();
        });

        modelBuilder.Entity<AgreementDeliverable>(entity =>
        {
            entity.ToTable("agreement_deliverables");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgreementId).HasColumnName("agreement_id").IsRequired();
            entity.Property(e => e.DeliverableName).HasColumnName("deliverable_name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("quantity").IsRequired();
            entity.Property(e => e.Configuration).HasColumnName("configuration").HasColumnType("jsonb");
            entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(1000);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.AgreementId);
        });

        modelBuilder.Entity<PlatformFeePayment>(entity =>
        {
            entity.ToTable("platform_fee_payments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingRequestId).HasColumnName("booking_request_id").IsRequired();
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.AgreementAmount).HasColumnName("agreement_amount").HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.FeePercentage).HasColumnName("fee_percentage").HasPrecision(5, 2).IsRequired();
            entity.Property(e => e.FeeAmount).HasColumnName("fee_amount").HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.RazorpayOrderId).HasColumnName("razorpay_order_id").HasMaxLength(100);
            entity.Property(e => e.RazorpayPaymentId).HasColumnName("razorpay_payment_id").HasMaxLength(100);
            entity.Property(e => e.RazorpaySignature).HasColumnName("razorpay_signature").HasMaxLength(500);
            entity.Property(e => e.PaidAt).HasColumnName("paid_at");
            entity.Property(e => e.FailureReason).HasColumnName("failure_reason").HasMaxLength(1000);
            entity.Property(e => e.DueDate).HasColumnName("due_date").IsRequired();
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => new { e.ProfessionalProfileId, e.Status });
            entity.HasIndex(e => e.BookingRequestId);
            entity.HasIndex(e => e.RazorpayOrderId);
        });

        modelBuilder.Entity<BookingStatusHistory>(entity =>
        {
            entity.ToTable("booking_status_history");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingRequestId).HasColumnName("booking_request_id").IsRequired();
            entity.Property(e => e.FromStatus).HasColumnName("from_status").HasConversion<string?>().HasMaxLength(30);
            entity.Property(e => e.ToStatus).HasColumnName("to_status").HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ChangedByType).HasColumnName("changed_by_type").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(2000);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.BookingRequestId);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.ToTable("conversations");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingRequestId).HasColumnName("booking_request_id").IsRequired();
            entity.Property(e => e.ConsumerId).HasColumnName("consumer_id").IsRequired();
            entity.Property(e => e.ProfessionalProfileId).HasColumnName("professional_profile_id").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.BookingRequestId).IsUnique();
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.ToTable("chat_messages");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id").IsRequired();
            entity.Property(e => e.SenderId).HasColumnName("sender_id").IsRequired();
            entity.Property(e => e.SenderType).HasColumnName("sender_type").HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.Content).HasColumnName("content").HasMaxLength(5000).IsRequired();
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.ReadAt).HasColumnName("read_at");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => new { e.ConversationId, e.CreatedAtUtc });
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppUserId).HasColumnName("app_user_id").IsRequired();
            entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(500).IsRequired();
            entity.Property(e => e.ExpiresAtUtc).HasColumnName("expires_at_utc").IsRequired();
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.RevokedAtUtc).HasColumnName("revoked_at_utc");
            entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip").HasMaxLength(50);
            entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token").HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.AppUserId);
        });
    }
}
