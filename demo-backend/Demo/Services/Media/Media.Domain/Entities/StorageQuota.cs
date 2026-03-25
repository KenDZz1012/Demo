using System.ComponentModel.DataAnnotations.Schema;
using Media.Domain.Common;

namespace Media.Domain.Entities;

[Table("storage_quotas")]
public class StorageQuota : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    /// <summary>Bytes used</summary>
    public long UsedBytes { get; set; } = 0;

    /// <summary>Max allowed bytes (default 8 GB)</summary>
    public long MaxBytes { get; set; } = 8_589_934_592L;
}
