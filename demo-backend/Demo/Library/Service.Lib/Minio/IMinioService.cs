using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Lib.BaseResponse;

namespace Service.Lib.Minio
{
    public interface IMinioService
    {
        Task<ApiResponse<MinioFile>> PostFileAsync(MinioFile file, string bucket);
    }
}
