using System.ComponentModel.DataAnnotations.Schema;
using Media.Domain.Common;

namespace Media.Domain.Entities;

[Table("media_files")]
public class MediaFile : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UploaderId { get; set; }

    public string OriginalName { get; set; } = null!;

    public string StoredName { get; set; } = null!;

    public string Url { get; set; } = null!;

    /// <summary>Bytes</summary>
    public long FileSize { get; set; }

    /// <summary>image/png, video/mp4, ...</summary>
    public string ContentType { get; set; } = null!;

    /// <summary>Pixels, nullable for non-image types</summary>
    public int? Width { get; set; }

    public int? Height { get; set; }

    /// <summary>Seconds, nullable for non-video types</summary>
    public double? Duration { get; set; }

    public string? Checksum { get; set; }

    /// <summary>MinIO bucket name</summary>
    public string Bucket { get; set; } = null!;
}
