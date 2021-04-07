using System;
using System.Collections.Generic;
using Amazon.S3.Model;

namespace S3.ConsoleApp.Services
{
    public sealed class Bucket
    {
        public Bucket()
        {
        }

        public Bucket(S3Bucket s3Bucket)
        {
            Name = s3Bucket.BucketName;
            CreationDate = s3Bucket.CreationDate;
        }

        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }

        public static IReadOnlyList<Bucket> ToReadOnlyList() => new List<Bucket>();

        public static explicit operator Bucket(S3Bucket s3Bucket)
        {
            return new Bucket(s3Bucket);
        }

    }
}