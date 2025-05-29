using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Channel.Infrastructure.Data;

[Table("ServerMember")]
public partial class ServerMember
{
    [Key]
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinedAt { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("ServerMembers")]
    public virtual Server Server { get; set; } = null!;
}
