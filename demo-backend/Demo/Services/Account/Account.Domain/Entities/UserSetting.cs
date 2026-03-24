using System.ComponentModel.DataAnnotations.Schema;
using Account.Domain.Common;

namespace Account.Domain.Entities;

[Table("user_settings")]
public class UserSetting : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    /// <summary>dark | light</summary>
    public string Theme { get; set; } = "dark";

    /// <summary>en-US, vi-VN, ...</summary>
    public string Language { get; set; } = "en-US";

    /// <summary>cozy | compact</summary>
    public string MessageDisplayMode { get; set; } = "cozy";

    public bool EnableAnimatedEmoji { get; set; } = true;

    public bool EnableGifAutoPlay { get; set; } = true;

    public bool EnableDeveloperMode { get; set; } = false;

    public virtual User User { get; set; } = null!;
}
