using Account.Domain.Common;
using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Data;

public partial class AccountContext : DbContext
{
    public AccountContext()
    {
    }

    public AccountContext(DbContextOptions<AccountContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<UserRelationship> UserRelationships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "uq_users_email").IsUnique();
            entity.HasIndex(e => e.UserName, "uq_users_username").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.UserName).HasMaxLength(50).HasColumnName("user_name");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.Status).HasMaxLength(20).HasColumnName("status");
            entity.Property(e => e.DisplayName).HasMaxLength(250).HasColumnName("display_name");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_profiles_pkey");
            entity.ToTable("user_profiles");

            entity.HasIndex(e => e.UserId, "uq_user_profiles_user_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Bio).HasMaxLength(500).HasColumnName("bio");
            entity.Property(e => e.BannerUrl).HasColumnName("banner_url");
            entity.Property(e => e.Pronouns).HasMaxLength(50).HasColumnName("pronouns");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Profile)
                .HasForeignKey<UserProfile>(d => d.UserId)
                .HasConstraintName("fk_user_profiles_user");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_settings_pkey");
            entity.ToTable("user_settings");

            entity.HasIndex(e => e.UserId, "uq_user_settings_user_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Theme).HasMaxLength(10).HasDefaultValue("dark").HasColumnName("theme");
            entity.Property(e => e.Language).HasMaxLength(10).HasDefaultValue("en-US").HasColumnName("language");
            entity.Property(e => e.MessageDisplayMode).HasMaxLength(10).HasDefaultValue("cozy").HasColumnName("message_display_mode");
            entity.Property(e => e.EnableAnimatedEmoji).HasDefaultValue(true).HasColumnName("enable_animated_emoji");
            entity.Property(e => e.EnableGifAutoPlay).HasDefaultValue(true).HasColumnName("enable_gif_auto_play");
            entity.Property(e => e.EnableDeveloperMode).HasDefaultValue(false).HasColumnName("enable_developer_mode");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Setting)
                .HasForeignKey<UserSetting>(d => d.UserId)
                .HasConstraintName("fk_user_settings_user");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<UserRelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_relationships_pkey");
            entity.ToTable("user_relationships");

            entity.HasIndex(e => e.AddresseeId, "idx_user_relationships_addressee");
            entity.HasIndex(e => e.RequesterId, "idx_user_relationships_requester");
            entity.HasIndex(e => new { e.RequesterId, e.AddresseeId }, "uq_user_relationships_pair").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.AddresseeId).HasColumnName("addressee_id");
            entity.Property(e => e.Status).HasMaxLength(20).HasColumnName("status");

            entity.HasOne(d => d.Addressee)
                .WithMany(p => p.UserRelationshipAddressees)
                .HasForeignKey(d => d.AddresseeId)
                .HasConstraintName("fk_user_relationships_addressee");

            entity.HasOne(d => d.Requester)
                .WithMany(p => p.UserRelationshipRequesters)
                .HasForeignKey(d => d.RequesterId)
                .HasConstraintName("fk_user_relationships_requester");

            ConfigureBaseEntity(entity);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private static void ConfigureBaseEntity<T>(EntityTypeBuilder<T> entity) where T : BaseEntity
    {
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
