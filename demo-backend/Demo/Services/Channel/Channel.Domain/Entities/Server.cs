using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Channel.Domain.Entities;

[Table("Server")]
public partial class Server
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public Guid OwnerId { get; set; }

    public string? IconUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Server")]
    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    [InverseProperty("Server")]
    public virtual ICollection<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
}
