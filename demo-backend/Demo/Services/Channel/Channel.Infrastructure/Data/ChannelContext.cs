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

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<ServerInviteLink> ServerInviteLinks { get; set; }

    public virtual DbSet<ServerMember> ServerMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Domain.Entities.Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channels_pkey");
            entity.ToTable("channels");

            entity.HasIndex(e => e.ServerId, "idx_channels_server_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Type).HasMaxLength(20).HasColumnName("type");

            entity.HasOne(d => d.Server)
                .WithMany(p => p.Channels)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_channels_server");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servers_pkey");
            entity.ToTable("servers");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.IconUrl).HasColumnName("icon_url");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<ServerInviteLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_invite_links_pkey");
            entity.ToTable("server_invite_links");

            entity.HasIndex(e => e.Code, "uq_server_invite_links_code").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.Code).HasMaxLength(255).HasColumnName("code");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");

            entity.HasOne(d => d.Server)
                .WithMany(p => p.ServerInviteLinks)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_server_invite_links_server");

            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<ServerMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_members_pkey");
            entity.ToTable("server_members");

            entity.HasIndex(e => e.UserId, "idx_server_members_user_id");
            entity.HasIndex(e => new { e.ServerId, e.UserId }, "uq_server_members_server_user").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Role).HasMaxLength(50).HasColumnName("role");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("joined_at");

            entity.HasOne(d => d.Server)
                .WithMany(p => p.ServerMembers)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_server_members_server");

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
