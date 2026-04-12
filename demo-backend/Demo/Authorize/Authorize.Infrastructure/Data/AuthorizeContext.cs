using Authorize.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Infrastructure.Data;

public partial class AuthorizeContext :  IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AuthorizeContext()
    {
    }

    public AuthorizeContext(DbContextOptions<AuthorizeContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Bắt buộc: đăng ký mapping bảng AspNetUsers, AspNetRoles, … của Identity.
        base.OnModelCreating(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
