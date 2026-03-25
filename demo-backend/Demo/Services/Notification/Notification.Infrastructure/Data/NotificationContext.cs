using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Common;
using Notification.Domain.Entities;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Infrastructure.Data;

public partial class NotificationContext : DbContext
{
    public NotificationContext()
    {
    }

    public NotificationContext(DbContextOptions<NotificationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NotificationEntity> Notifications { get; set; }
    public virtual DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }
    public virtual DbSet<PushSubscription> PushSubscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");
            entity.ToTable("notifications");

            entity.HasIndex(e => e.UserId, "idx_notifications_user_id");
            entity.HasIndex(e => new { e.UserId, e.IsRead }, "idx_notifications_user_unread");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Type).HasMaxLength(50).HasColumnName("type");
            entity.Property(e => e.Title).HasMaxLength(200).HasColumnName("title");
            entity.Property(e => e.Body).HasMaxLength(1000).HasColumnName("body");
            entity.Property(e => e.ReferenceType).HasMaxLength(50).HasColumnName("reference_type");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<UserNotificationSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_notification_settings_pkey");
            entity.ToTable("user_notification_settings");

            entity.HasIndex(e => new { e.UserId, e.GuildId, e.ChannelId }, "uq_notification_settings_scope").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.MutedUntil).HasColumnName("muted_until");
            entity.Property(e => e.MessageNotifications).HasMaxLength(20).HasColumnName("message_notifications");
            entity.Property(e => e.SuppressEveryone).HasColumnName("suppress_everyone");
            entity.Property(e => e.SuppressRoles).HasColumnName("suppress_roles");
            entity.Property(e => e.MobilePush).HasColumnName("mobile_push");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<PushSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("push_subscriptions_pkey");
            entity.ToTable("push_subscriptions");

            entity.HasIndex(e => e.UserId, "idx_push_subscriptions_user_id");
            entity.HasIndex(e => e.Endpoint, "uq_push_subscriptions_endpoint").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Endpoint).HasColumnName("endpoint");
            entity.Property(e => e.P256dhKey).HasColumnName("p256dh_key");
            entity.Property(e => e.AuthKey).HasColumnName("auth_key");
            entity.Property(e => e.Platform).HasMaxLength(20).HasColumnName("platform");
            ConfigureBaseEntity(entity);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private static void ConfigureBaseEntity<T>(EntityTypeBuilder<T> entity) where T : BaseEntity
    {
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()").HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
