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
            entity.Property(e => e.ProfessionalRole).HasColumnName("professional_role").HasConversion<string?>().HasMaxLength(40);
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(e => e.UpdatedAtUtc).HasColumnName("updated_at_utc");

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.UserType);
            entity.HasIndex(e => e.ProfessionalRole);
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
    }
}
