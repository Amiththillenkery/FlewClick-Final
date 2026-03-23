using FlewClick.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Persistence;

public class FlewClickDbContext : DbContext
{
    public FlewClickDbContext(DbContextOptions<FlewClickDbContext> options) : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();

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
    }
}
