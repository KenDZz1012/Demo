using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Channel.Domain.Entities;

[Table("Server")]
public partial class Server
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid OwnerId { get; set; }

    public string? IconUrl { get; set; }
    
    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual ICollection<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
    
    public virtual ICollection<ServerInviteLink> ServerInviteLinks { get; set; } = new List<ServerInviteLink>();

    public Guid? CreatedBy { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public Guid? UpdatedBy { get; set; }
        
    public DateTime? UpdatedAt { get; set; }
    
    public Guid? DeletedBy { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}
