using Media.Domain.Common;
using Media.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Media.Infrastructure.Data;

public partial class MediaContext : DbContext
{
    public MediaContext()
    {
    }

    public MediaContext(DbContextOptions<MediaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MediaFile> MediaFiles { get; set; }
    public virtual DbSet<StorageQuota> StorageQuotas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<MediaFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("media_files_pkey");
            entity.ToTable("media_files");

            entity.HasIndex(e => e.UploaderId, "idx_media_files_uploader_id");
            entity.HasIndex(e => e.Checksum, "idx_media_files_checksum");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.UploaderId).HasColumnName("uploader_id");
            entity.Property(e => e.OriginalName).HasMaxLength(500).HasColumnName("original_name");
            entity.Property(e => e.StoredName).HasMaxLength(500).HasColumnName("stored_name");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.ContentType).HasMaxLength(100).HasColumnName("content_type");
            entity.Property(e => e.Width).HasColumnName("width");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Checksum).HasMaxLength(64).HasColumnName("checksum");
            entity.Property(e => e.Bucket).HasMaxLength(100).HasColumnName("bucket");
            ConfigureBaseEntity(entity);
        });

        modelBuilder.Entity<StorageQuota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("storage_quotas_pkey");
            entity.ToTable("storage_quotas");

            entity.HasIndex(e => e.UserId, "uq_storage_quotas_user_id").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UsedBytes).HasColumnName("used_bytes");
            entity.Property(e => e.MaxBytes).HasDefaultValue(8_589_934_592L).HasColumnName("max_bytes");
            ConfigureBaseEntity(entity);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private static void ConfigureBaseEntity<T>(EntityTypeBuilder<T> entity) where T : BaseEntity
    {
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()").HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
