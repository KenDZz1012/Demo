using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using Service.Lib.BaseResponse;

namespace Service.Lib.Minio
{
    public class MinioService : IMinioService
    {
        private readonly MinioContext _minioContext;

        public MinioService(MinioContext minioContext)
        {
            _minioContext = minioContext;
        }

        public async Task<ApiResponse<MinioFile>> PostFileAsync(MinioFile file, string bucket)
        {
            try
            {
                var minioConnection = _minioContext.CreateConnection();
                var bucketName = bucket?.ToLower() ?? "new_bucket";
                var objectName = file.FileName;
                var existsBucket = await minioConnection.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!existsBucket)
                {
                    await minioConnection.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }
                await minioConnection.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(file.formFile)
                    .WithObjectSize(file.formFile.Length));
                string Url = await minioConnection.PresignedGetObjectAsync(new PresignedGetObjectArgs().WithBucket(bucket).WithObject(objectName).WithExpiry(7 * 24 * 3600));
                var uri = new Uri(Url);
                MinioFile minioFile = new MinioFile
                {
                    FileName = file.FileName,
                    FilePath = uri.Scheme + "://" + uri.Host + ":" + uri.Port + uri.AbsolutePath,
                    Size = file.Size,
                };
                return ApiResponse<MinioFile>.Success(minioFile, "Upload file thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<MinioFile>.Failure("500", ex.Message);
            }
        }
    }
}
