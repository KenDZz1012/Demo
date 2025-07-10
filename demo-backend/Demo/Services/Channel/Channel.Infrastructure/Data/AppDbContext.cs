using System;
using System.Collections.Generic;
using Channel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Channel.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Domain.Entities.Channel> Channels { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<ServerMember> ServerMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=103.82.25.49,1433;Database=Channel;User Id=sa;Password=Kienteo1012;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Channel__3214EC07CAA22EA6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Server).WithMany(p => p.Channels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Channel__ServerI__2A4B4B5E");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Server__3214EC0799217EED");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ServerMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServerMe__3214EC07654A5CE6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Server).WithMany(p => p.ServerMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServerMem__Serve__2F10007B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
