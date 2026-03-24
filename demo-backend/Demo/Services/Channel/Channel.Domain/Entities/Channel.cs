using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("channels")]
public partial class Channel : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    public string? Name { get; set; }

    /// <summary>Text | Voice</summary>
    public string? Type { get; set; }

    public virtual Server Server { get; set; } = null!;
}
