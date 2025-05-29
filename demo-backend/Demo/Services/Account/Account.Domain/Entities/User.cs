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
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [StringLength(250)]
    public string DisplayName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    public bool IsAdmin { get; set; }

    [InverseProperty("Addressee")]
    public virtual ICollection<UserRelationship> UserRelationshipAddressees { get; set; } = new List<UserRelationship>();

    [InverseProperty("Requester")]
    public virtual ICollection<UserRelationship> UserRelationshipRequesters { get; set; } = new List<UserRelationship>();
}

