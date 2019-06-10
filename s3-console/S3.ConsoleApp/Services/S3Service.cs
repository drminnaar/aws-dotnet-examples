using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace S3.ConsoleApp.Services
{
    public sealed class S3Service : IS3Service
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IAmazonS3 amazonS3, ILogger<S3Service> logger)
        {
            _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<CreatedBucket> CreateBucketAsync(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("A bucket name is required", nameof(bucketName));

            return createBucketAsync();

            async Task<CreatedBucket> createBucketAsync()
            {
                try
                {
                    if (await _amazonS3.DoesS3BucketExistAsync(bucketName))
                    {
                        _logger.LogWarning($"A bucket having name '{bucketName}' already exists.");
                        return null;
                    }

                    var response = await _amazonS3.PutBucketAsync(bucketName);

                    if (response.HttpStatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogWarning($"A bucket having name '{bucketName}' could not be created.");
                        return null;
                    }
                    
                    return new CreatedBucket
                    {
                        Name = bucketName
                    };
                }
                catch (AmazonS3Exception e)
                {
                    _logger.LogError(e, e.Message);
                    throw;
                }
            }
        }

        public Task<DeletedBucket> DeleteBucketAsync(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("A bucket name is required", nameof(bucketName));

            return removeBucketAsync();

            async Task<DeletedBucket> removeBucketAsync()
            {
                try
                {
                    if (!await _amazonS3.DoesS3BucketExistAsync(bucketName))
                    {
                        _logger.LogWarning($"A bucket having name '{bucketName}' does not exist.");
                        return null;
                    }

                    var response = await _amazonS3.DeleteBucketAsync(bucketName);

                    if (response.HttpStatusCode != HttpStatusCode.NoContent)
                    {
                        _logger.LogWarning($"A bucket having name '{bucketName}' could not be deleted ({response.HttpStatusCode}).");
                        return null;
                    }
                    
                    return new DeletedBucket
                    {
                        Name = bucketName
                    };
                }
                catch (AmazonS3Exception e)
                {
                    _logger.LogError(e, e.Message);
                    throw;
                }
            }
        }

        public async Task<IReadOnlyList<Bucket>> ListAllBucketsAsync()
        {
            try
            {
                var response = await _amazonS3.ListBucketsAsync();

                return response
                    ?.Buckets
                    ?.Select(b => (Bucket)b)
                    ?.ToList()
                    ?? Bucket.ToReadOnlyList();
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}