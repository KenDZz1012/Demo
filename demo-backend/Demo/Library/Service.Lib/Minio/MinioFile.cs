using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.Minio
{
    public class MinioFile
    {
        public Stream formFile { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public double Size { get; set; }
    }
}
