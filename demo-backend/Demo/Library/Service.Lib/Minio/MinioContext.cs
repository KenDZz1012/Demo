using System;
using Minio;

namespace Service.Lib.Minio
{
    public class MinioContext
    {
        public string Endpoint { get; private set; }
        public int Port { get; private set; }
        private string AccessKey { get; set; }
        private string SecretKey { get; set; }
        public bool Https { get; private set; }

        public MinioContext()
        {
            string config = Environment.GetEnvironmentVariable("MINIO_CONNECTION");
            Console.WriteLine($"MINIO_CONNECTION: {config}");

            try
            {
                string[] splitConfig = config.Split(';');
                Endpoint = splitConfig[0];
                Port = Convert.ToInt32(splitConfig[1]);
                AccessKey = splitConfig[2];
                SecretKey = splitConfig[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("MinioContext Init Error: " + ex.Message);
            }
        }

        public IMinioClient CreateConnection()
        {
            return new MinioClient()
                .WithEndpoint(Endpoint, Port)
                .WithCredentials(AccessKey, SecretKey)
                .Build();
        }
    }
}
