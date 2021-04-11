![](https://repository-images.githubusercontent.com/190967041/5bfaad80-9b0c-11eb-90ab-c0df5517524c)

# AWS for .NET Developers

A collection of independent .NET projects written in C# .NET 5 that demonstrate how to integrate with various AWS services using the AWS SDK for dotnet.

## Projects

* [Cognito Mvc] - This project demonstrates how to integrate an ASP.NET Core MVC web application with Amazon Cognito using the Amazon SDK.

    The following features have been implemented:

  * Signup for a new account
  * Confirm Signup (using confirmation code)
  * Sign into account
  * Sign out of account

* [Cognito Api] - This project demonstrates how to integrate a .NET Core Web Api application with Amazon Cognito using the Amazon SDK.

  This Web Api application provides the following endpoints:

  * Signups
    * POST api/signups - Allows an api consumer to signup for a new account
  * Tokens
    * POST api/tokens - Provides JWT access token for valid account credentials
  * Values
    * GET api/values - An unauthorized endpoint that returns a list of values
    * GET api/values/123 - An authorized endpoint (Requires JWT access token) that returns a single value

* [S3 ConsoleApp] - This project demonstrates how to integrate a .NET Core Console application with Amazon S3 using the Amazon SDK.

    The following features have been implemented:

  * List all S3 buckets
  * Create new S3 bucket
  * Delete S3 bucket

* [DynamoDb ConsoleApp] - This project demonstrates how to integrate a .NET Core console application with DynamoDb using the Amazon SDK. This application provides the functionality required to both manage DynamoDb tables, and manage the data stored in a DynamoDB table

    The following features have been implemented:

  * Manage Tables
    * Create table
    * List and find tables
    * Wait for tables to be described (eventually consistent)
    * Delete table
  * Manage Book Table Data
    * Automatically creates Book table
    * Add books to Book table
    * Update books in Book table
    * Delete books from Book table
    * List and find books in Book table

* [SNS ConsoleApp] - This project demonstrates how to integrate a .NET Core console application with SNS using the Amazon SDK. This application provides the functionality required to manage SNS topcs, subscriptions, and publications.

    The following features have been implemented:

  * Manage Topics
    * Create topic
    * List and find topics  
    * Delete topic
  * Manage Subscriptions
    * Create an email subscription
    * Cancel a subscription
    * List subscriptions
  * Manage Publications
    * An example showing how to publish a 'Game Ranking' to a 'game-ranking' topic

* [SQS ConsoleApp] - This project demonstrates how to integrate a .NET Core console application with SQS using the Amazon SDK.

    The following features have been implemented:

  * Manage Queues
    * Create queue
    * List queues
    * Delete queue
    * Get queue url
  * Manage Game Ranking Queue
    * Enqueue game ranking to queue
    * Dequeue game rankings from queue

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/drminnaar/aws-dotnetcore-examples/tags).

## Authors

* **Douglas Minnaar** - *Initial work* - [drminnaar](https://github.com/drminnaar)

[Cognito Mvc]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/cognito-mvc-web
[Cognito Api]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/cognito-mvc-api
[S3 ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/s3-console
[DynamoDb ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/dynamodb-console
[SNS ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/sns-console
[SQS ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/sqs-console
[Lambda HelloWorld]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/lambda/HelloWorld
[Creating .NET Core AWS Lambda Projects without Visual Studio]: https://aws.amazon.com/blogs/developer/creating-net-core-aws-lambda-projects-without-visual-studio/
