using Channel.Domain.Common;
using Channel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Channel.Infrastructure.Data;

public partial class ChannelContext : DbContext
{
    public ChannelContext()
    {
    }

    public ChannelContext(DbContextOptions<ChannelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Domain.Entities.Channel> Channels { get; set; }
    public virtual DbSet<ChannelCategory> ChannelCategories { get; set; }
    public virtual DbSet<ChannelPermissionOverride> ChannelPermissionOverrides { get; set; }
    public virtual DbSet<ChannelThread> ChannelThreads { get; set; }
    public virtual DbSet<ThreadMember> ThreadMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<ChannelCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channel_categories_pkey");
            entity.ToTable("channel_categories");

            entity.HasIndex(e => e.GuildId, "idx_channel_categories_guild_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Position).HasDefaultValue(0).HasColumnName("position");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<Domain.Entities.Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channels_pkey");
            entity.ToTable("channels");

            entity.HasIndex(e => e.GuildId, "idx_channels_guild_id");
            entity.HasIndex(e => e.CategoryId, "idx_channels_category_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Type).HasMaxLength(20).HasColumnName("type");
            entity.Property(e => e.Position).HasDefaultValue(0).HasColumnName("position");
            entity.Property(e => e.Topic).HasMaxLength(1024).HasColumnName("topic");
            entity.Property(e => e.Nsfw).HasColumnName("nsfw");
            entity.Property(e => e.RateLimit).HasDefaultValue(0).HasColumnName("rate_limit");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.UserLimit).HasColumnName("user_limit");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Channels)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_channels_category");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<ChannelPermissionOverride>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channel_permission_overrides_pkey");
            entity.ToTable("channel_permission_overrides");

            entity.HasIndex(e => new { e.ChannelId, e.TargetType, e.TargetId },
                "uq_channel_permission_overrides").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.TargetType).HasMaxLength(10).HasColumnName("target_type");
            entity.Property(e => e.TargetId).HasColumnName("target_id");
            entity.Property(e => e.Allow).HasDefaultValue(0L).HasColumnName("allow");
            entity.Property(e => e.Deny).HasDefaultValue(0L).HasColumnName("deny");

            entity.HasOne(d => d.Channel)
                .WithMany(p => p.PermissionOverrides)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_permission_overrides_channel");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<ChannelThread>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channel_threads_pkey");
            entity.ToTable("channel_threads");

            entity.HasIndex(e => e.ChannelId, "idx_channel_threads_channel_id");
            entity.HasIndex(e => e.OwnerId, "idx_channel_threads_owner_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Archived).HasColumnName("archived");
            entity.Property(e => e.Locked).HasColumnName("locked");
            entity.Property(e => e.AutoArchiveDuration).HasDefaultValue(1440).HasColumnName("auto_archive_duration");

            entity.HasOne(d => d.Channel)
                .WithMany(p => p.Threads)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_channel_threads_channel");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<ThreadMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("thread_members_pkey");
            entity.ToTable("thread_members");

            entity.HasIndex(e => new { e.ThreadId, e.UserId }, "uq_thread_members").IsUnique();
            entity.HasIndex(e => e.UserId, "idx_thread_members_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ThreadId).HasColumnName("thread_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("joined_at");

            entity.HasOne(d => d.Thread)
                .WithMany(p => p.ThreadMembers)
                .HasForeignKey(d => d.ThreadId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_thread_members_thread");

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
