using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Account.Domain.Common.Constants;
using Microsoft.EntityFrameworkCore;

namespace Account.Domain.Entities;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D10534C36415AC", IsUnique = true)]
public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string DisplayName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<UserRelationship> UserRelationshipAddressees { get; set; } = new List<UserRelationship>();

    public virtual ICollection<UserRelationship> UserRelationshipRequesters { get; set; } = new List<UserRelationship>();
}

