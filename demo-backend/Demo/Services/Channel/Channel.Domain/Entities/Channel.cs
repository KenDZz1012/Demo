using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Channel.Infrastructure.Data;

[Table("Channel")]
public partial class Channel
{
    [Key]
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(20)]
    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("Channels")]
    public virtual Server Server { get; set; } = null!;
}
