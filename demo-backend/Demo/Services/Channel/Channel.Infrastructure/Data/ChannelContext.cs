using System;
using System.Collections.Generic;
using Channel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=103.82.25.49;Port=6543;Database=channel;Username=kendz;Password=kienteo1012");

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
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.Server).WithMany(p => p.Channels)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("fk_channels_server");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servers_pkey");

            entity.ToTable("servers");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IconUrl).HasColumnName("icon_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
        });

        modelBuilder.Entity<ServerInviteLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_invite_link_pkey");

            entity.ToTable("server_invite_link");

            entity.HasIndex(e => e.Code, "server_invite_link_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Expiredat).HasColumnName("expiredat");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Serverid).HasColumnName("serverid");

            entity.HasOne(d => d.Server).WithMany(p => p.ServerInviteLinks)
                .HasForeignKey(d => d.Serverid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_server");
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
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("joined_at");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Server).WithMany(p => p.ServerMembers)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("fk_server_members_server");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
