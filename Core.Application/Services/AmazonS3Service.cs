using Amazon.S3;
using Amazon.S3.Model;
using Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private const string BUCKET_NAME = "thesis-supermarket";
        private const string accessKey = "06BEECD0B86D85C80E08";
        private const string secretKey = "uYPF8fhjxQyMVEtHBSppnfb4xkWqmfRlHPrQ5wCH";

        public AmazonS3Service()
        {
            var config = new AmazonS3Config
            {
                ServiceURL = "https://s3.filebase.com",
                ForcePathStyle = true,
            };
            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        }

        public async Task<string> UploadFileAsync(string pPath, IFormFile pFile)
        {
            try
            {
                var key = $"Uploads/{pPath}/{pFile.FileName}";

                // Tải file lên S3
                await UploadObjectFromFileAsync(pFile, BUCKET_NAME, key);

                string url = await GetFileCidFromS3Async(key);

                return url;
            }
            catch (AmazonS3Exception ex)
            {
                // Xử lý lỗi và ném ra ngoại lệ
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }

        private async Task UploadObjectFromFileAsync(IFormFile file, string bucketName, string key)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = file.ContentType,
                    };

                    putRequest.Metadata.Add("x-amz-meta-title", file.FileName);

                    // Tải file lên S3
                    PutObjectResponse response = await _s3Client.PutObjectAsync(putRequest);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public async Task<string> GetFileCidFromS3Async(string key)
        {
            try
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = key
                };

                using (var response = await _s3Client.GetObjectAsync(getRequest))
                {
                    if (response.Metadata.Keys.Contains("x-amz-meta-cid"))
                    {
                        var cid = response.Metadata["x-amz-meta-cid"];
                        return "https://ipfs.filebase.io/ipfs/" + cid;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
