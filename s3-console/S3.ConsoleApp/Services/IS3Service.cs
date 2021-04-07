using System.Collections.Generic;
using System.Threading.Tasks;

namespace S3.ConsoleApp.Services
{
    public interface IS3Service
    {
        Task<CreatedBucket?> CreateBucketAsync(string bucketName);
        Task<DeletedBucket?> DeleteBucketAsync(string bucketName);
        Task<IReadOnlyList<Bucket>> ListAllBucketsAsync();
    }
}