# S3 ConsoleApp README

This project demonstrates how to integrate a .NET5 Console application with Amazon S3 using the Amazon SDK.

## Features

The following features have been implemented:

* List all S3 buckets
* Create new S3 bucket
* Delete S3 bucket

## Getting Started

### Configure User Secrets

```powershell
# change directory to S3.ConsoleApp
cd ./S3.ConsoleApp

# initialize user secrets
dotnet user-secrets init

# add AWS configuration options to user secrets
dotnet user-secrets set "AWS:Profile" "<YOUR_CHOSEN_PROFILE_NAME>"
dotnet user-secrets set "AWS:Region" "<YOUR_CHOSEN_REGION_NAME>"

# verify that secrets were added correctly and successfully
dotnet user-secrets list

AWS:Region = <YOUR_CHOSEN_REGION_NAME>
AWS:Profile = <YOUR_CHOSEN_PROFILE_NAME>

# clear secrets if you no longer need them
dotnet user-secrets clear
```

### Start Application

```powershell
# change directory to S3.ConsoleApp
cd ./S3.ConsoleApp

# run app
dotnet run
```

![aws-s3-demo](https://user-images.githubusercontent.com/33935506/114326429-da953080-9b88-11eb-8ef0-aa4c1ec554bf.png)

## Walkthrough

### AWS SDK Setup

```csharp
// FILE: Program.cs
// https://github.com/drminnaar/aws-dotnet-examples/blob/master/s3-console/S3.ConsoleApp/Program.cs

// configure AWS services

var services = new ServiceCollection();

// configure aws options (uses user secrets)
services.AddDefaultAWSOptions(configuration.GetAWSOptions());

// configure AWS S3 service
services.AddAWSService<IAmazonS3>();
```

### Create Bucket

```csharp
// FILE: S3Service.cs
// https://github.com/drminnaar/aws-dotnet-examples/blob/master/s3-console/S3.ConsoleApp/Services/S3Service.cs

async Task<CreatedBucket?> createBucketAsync()
{
    try
    {
        // check if bucket exists
        if (await _amazonS3.DoesS3BucketExistAsync(bucketName))
        {
            _logger.LogWarning($"A bucket having name '{bucketName}' already exists.");
            return null;
        }

        // create bucket
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
```

### List Buckets

```csharp
// FILE: S3Service.cs
// https://github.com/drminnaar/aws-dotnet-examples/blob/master/s3-console/S3.ConsoleApp/Services/S3Service.cs

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
```

### Delete Bucket

```csharp
// FILE: S3Service.cs
// https://github.com/drminnaar/aws-dotnet-examples/blob/master/s3-console/S3.ConsoleApp/Services/S3Service.cs

async Task<DeletedBucket?> removeBucketAsync()
{
    try
    {
        // check if bucket exists
        if (!await _amazonS3.DoesS3BucketExistAsync(bucketName))
        {
            _logger.LogWarning($"A bucket having name '{bucketName}' does not exist.");
            return null;
        }

        // delete bucket
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
```

---

## Notable Nuget Packages

* [AWSSDK.S3] - Amazon Simple Storage Service (Amazon S3), provides developers and IT teams with secure, durable, highly-scalable object storage.
* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

---

[AWSSDK.S3]: https://www.nuget.org/packages/AWSSDK.S3/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/