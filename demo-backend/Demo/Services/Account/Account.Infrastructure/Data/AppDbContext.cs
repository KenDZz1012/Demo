using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Account.Domain.Entities;

namespace Account.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<UserRelationship> UserRelationships { get; set; }

        // Optional: cấu hình entity bằng Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(u => u.ID);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.AvatarUrl).HasMaxLength(255);
                entity.Property(u => u.Status).IsRequired().HasMaxLength(200);
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired();
            });

            // UserRelationship entity
            modelBuilder.Entity<UserRelationship>(entity =>
            {
                entity.ToTable("User_Relationship");
                entity.HasKey(ur => ur.ID);
                entity.Property(ur => ur.RequesterId).IsRequired();
                entity.Property(ur => ur.AddresseeId).IsRequired();
                entity.Property(ur => ur.Status).IsRequired();
                entity.Property(ur => ur.CreatedAt).IsRequired();
            });
        }
    }
}
