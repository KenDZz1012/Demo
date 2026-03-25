using Guild.Domain.Common;
using Guild.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GuildEntity = Guild.Domain.Entities.Guild;

namespace Guild.Infrastructure.Data;

public partial class GuildContext : DbContext
{
    public GuildContext()
    {
    }

    public GuildContext(DbContextOptions<GuildContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GuildEntity> Guilds { get; set; }
    public virtual DbSet<GuildMember> GuildMembers { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<MemberRole> MemberRoles { get; set; }
    public virtual DbSet<GuildBan> GuildBans { get; set; }
    public virtual DbSet<GuildInvite> GuildInvites { get; set; }
    public virtual DbSet<GuildEmoji> GuildEmojis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<GuildEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guilds_pkey");
            entity.ToTable("guilds");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Description).HasMaxLength(500).HasColumnName("description");
            entity.Property(e => e.IconUrl).HasColumnName("icon_url");
            entity.Property(e => e.BannerUrl).HasColumnName("banner_url");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.VerificationLevel).HasMaxLength(20).HasColumnName("verification_level");
            entity.Property(e => e.MaxMembers).HasDefaultValue(500000).HasColumnName("max_members");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<GuildMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guild_members_pkey");
            entity.ToTable("guild_members");

            entity.HasIndex(e => new { e.GuildId, e.UserId }, "uq_guild_members_guild_user").IsUnique();
            entity.HasIndex(e => e.UserId, "idx_guild_members_user_id");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Nickname).HasMaxLength(100).HasColumnName("nickname");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("now()").HasColumnName("joined_at");
            entity.Property(e => e.IsMuted).HasColumnName("is_muted");
            entity.Property(e => e.IsDeafened).HasColumnName("is_deafened");

            entity.HasOne(d => d.Guild)
                .WithMany(p => p.GuildMembers)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guild_members_guild");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");
            entity.ToTable("roles");

            entity.HasIndex(e => e.GuildId, "idx_roles_guild_id");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Color).HasMaxLength(10).HasColumnName("color");
            entity.Property(e => e.Permissions).HasColumnName("permissions");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Hoist).HasColumnName("hoist");
            entity.Property(e => e.Mentionable).HasColumnName("mentionable");

            entity.HasOne(d => d.Guild)
                .WithMany(p => p.Roles)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_roles_guild");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<MemberRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_roles_pkey");
            entity.ToTable("member_roles");

            entity.HasIndex(e => new { e.MemberId, e.RoleId }, "uq_member_roles_pair").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Member)
                .WithMany(p => p.MemberRoles)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_member_roles_member");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.MemberRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_member_roles_role");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<GuildBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guild_bans_pkey");
            entity.ToTable("guild_bans");

            entity.HasIndex(e => new { e.GuildId, e.UserId }, "uq_guild_bans_guild_user").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Reason).HasMaxLength(500).HasColumnName("reason");

            entity.HasOne(d => d.Guild)
                .WithMany(p => p.GuildBans)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guild_bans_guild");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<GuildInvite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guild_invites_pkey");
            entity.ToTable("guild_invites");

            entity.HasIndex(e => e.Code, "uq_guild_invites_code").IsUnique();
            entity.HasIndex(e => e.GuildId, "idx_guild_invites_guild_id");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Code).HasMaxLength(20).HasColumnName("code");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.MaxUses).HasColumnName("max_uses");
            entity.Property(e => e.Uses).HasColumnName("uses");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");

            entity.HasOne(d => d.Guild)
                .WithMany(p => p.GuildInvites)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guild_invites_guild");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<GuildEmoji>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guild_emojis_pkey");
            entity.ToTable("guild_emojis");

            entity.HasIndex(e => e.GuildId, "idx_guild_emojis_guild_id");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.Animated).HasColumnName("animated");
            entity.Property(e => e.Available).HasColumnName("available");

            entity.HasOne(d => d.Guild)
                .WithMany(p => p.GuildEmojis)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_guild_emojis_guild");
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
