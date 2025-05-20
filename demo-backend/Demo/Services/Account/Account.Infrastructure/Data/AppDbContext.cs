using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Account.Domain.Entities;

namespace Account.Infrastructure.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<UserRelationship> UserRelationships { get; set; }

        // Optional: cấu hình entity bằng Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User__3214EC27C034B329");

                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534C36415AC").IsUnique();

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("ID");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
                entity.Property(e => e.DisplayName).HasMaxLength(250);
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRelationship>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User_Rel__3214EC27C64B69F7");

                entity.ToTable("User_Relationship");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("ID");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Addressee).WithMany(p => p.UserRelationshipAddressees)
                    .HasForeignKey(d => d.AddresseeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Rela__Addre__3F466844");

                entity.HasOne(d => d.Requester).WithMany(p => p.UserRelationshipRequesters)
                    .HasForeignKey(d => d.RequesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Rela__Reque__3E52440B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
