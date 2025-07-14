using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Channel.Domain.Entities;

[Table("Channel")]
public partial class Channel
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Server Server { get; set; } = null!;
}
