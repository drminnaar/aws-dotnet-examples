![aws-dotnet-examples-github](https://user-images.githubusercontent.com/33935506/114672166-f0595000-9d58-11eb-8240-65c0b06493b5.png)

# AWS for .NET Developers

A collection of independent .NET projects written in C# .NET 5 that demonstrate how to integrate with various AWS services using the AWS SDK for dotnet.

* [Toolchain](#toolchain)
* [Projects](#projects)
* [AWS CLI](#aws-cli)
  * [Installing AWS CLI](#installing-aws-cli)
  * [List AWS CLI Configuration Data](#list-aws-cli-configuration-data)
  * [Configure AWS CLI](#configure-aws-cli)
  * [Configure Additional AWS CLI Profiles](#configure-additional-aws-cli-profiles)
* [AWS Project Configuration](aws-project-configuration)
  * [Using appsettings](#using-appsettings)
  * [Using Environment Variables](#using-environment-variables)
  * [Using dotnet User Secrets](#using-dotnet-user-secrets)
* [Signup With AWS Free Tier](#signup-with-aws-free-tier)
* [Learning](#learning)
* [Certification](#certification)

---

## Toolchain

All projects have been built or tested on *Windows 10* and *Ubuntu 20.04*. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.
* [Ubuntu 20.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.
* [Amazon S3] - An AWS service that provides authentication, authorization, and user management for your web and mobile apps.
* [Amazon SNS] - Amazon Simple Notification Service (Amazon SNS) is a web service that coordinates and manages the delivery or sending of messages to subscribing endpoints or clients.
* [Amazon SQS] - Amazon Simple Queue Service (SQS) is a fully managed message queuing service that enables you to decouple and scale microservices, distributed systems, and serverless applications.
* [Amazon Cognito] - Amazon Simple Notification Service (Amazon SNS) is a web service that coordinates and manages the delivery or sending of messages to subscribing endpoints or clients.
* [Amazon DynamoDb] - Amazon DynamoDB is a fully managed NoSQL database service that provides fast and predictable performance with seamless scalability.
* [.NET5] - .NET5 is a free and open-source managed software framework for Linux, Windows and macOS.
* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

---

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

---

## AWS CLI

Even as a C#.NET developer, you will come to find the AWS CLI to be one of your most valuable tools. See the [official AWS CLI documentation](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-welcome.html) to learn more.

### Installing AWS CLI

Please install **Version 2** of the AWS CLI.

For installation instructions for all the major platforms, please visit the [official AWS CLI installation documentation](https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2.html)

### List AWS CLI Configuration Data

From your terminal window, type the following commands:

```powershell
# list aws cli configuration data
aws configure list

# list aws cli configuration profiles (only available from version 2)
aws configure list-profiles
```

### Configure AWS CLI

Type the following command to create a default profile:

```powershell
aws configure

AWS Access Key ID [None]: AKIAIOSFODNN7EXAMPLE
AWS Secret Access Key [None]: wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
Default region name [None]: us-west-2
Default output format [None]: json
```

AWS configuration can be found in your home folder at the following locations:

```powershell
# windows
~\aws\credentials
~\aws\config

# linux / macOS
~/aws/credentials
~/aws/config
```

### Configure Additional AWS CLI Profiles

Type the following command to create additional profiles:

```powershell
aws configure --profile s3demo
aws configure --profile sqsdemo
aws configure --profile dynamodbdemo
```

---

## AWS Project Configuration

We need to provide some AWS configuration before we can start using the AWS SDK in our projects. In this section, I provide 3 options that one can use to provide the configuration that the AWS SDK requires at runtime.

* provide configuration in **appsettings.json** file
* provide configuration using **environment variables**
* provide configuration using **dotnet user secrets**

It is also considered bad practice to store any AWS configuration in the application configuration files. Therefore, the 2 preferred approaches to storing your AWS configuration are as follows:

* provide configuration using environment variables
* provide configuration using dotnet user secrets

> **IMPORTANT**
>
> NEVER store any AWS credentials \(access\__key\_id, secret_\__access\__key\_id\) in configuration files

### Using appsettings

This approach is not considered the best approach as it uses an application configuration file that can be committed to source control with sensitive credentials.

> **NOTE**
>
> This is the current approach for all projects, but will be changed to use "user secrets" in the future

Add the following configuration section to your `appsettings.json` file:

```javascript
// appsettings.json

{
    "AWS": {
        "Region": "us-east-1",
        "Profile": "aws-demo"
    }
}
```

### Using Environment Variables

From your terminal, add the following environment variables:

* AWS\_PROFILE
* AWS\_REGION

```powershell
# Powershell

$env:AWS_PROFILE = "aws-demo"
$env:AWS_REGION = "us-east-1"
```

```powershell
# Linux / macOS

AWS_PROFILE='aws-demo'
AWS_REGION='us-east-1'
```

```bash
# Windows CMD

set AWS_PROFILE=aws-demo
set AWS_REGION=us-east-1
```

### Using dotnet User Secrets

> **NOTE**
>
> This is the approach that will be used for all projects in the future.

Working with user secrets involves using the _Secret Manager_ tool. According to the official Microsoft documentation:

> The Secret Manager tool stores sensitive data during the development of an ASP.NET Core project. In this context, a piece of sensitive data is an app secret. App secrets are stored in a separate location from the project tree. The app secrets are associated with a specific project or shared across several projects. The app secrets aren't checked into source control

For more information, [see the official documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

> **WARNING**
>
> The Secret Manager tool doesn't encrypt the stored secrets and shouldn't be treated as a trusted store. It's for development purposes only. The keys and values are stored in a JSON configuration file in the user profile directory.

#### Step 1 - Initialise User Secrets

From the terminal, type the following command to initialize user-secrets,

```bash
cd ./Aws.S3Demo.ConsoleApp
dotnet user-secrets init
```

The following entry is added to the project file:

```xml
<PropertyGroup>
  <UserSecretsId>567f0dg2-06b1-5567-94a5-8c855a1a7972</UserSecretsId>
</PropertyGroup>
```

Use the following commands to access the user secrets location and ***secrets.json*** file:

```powershell
# Powershell

cat $env:APPDATA\Microsoft\UserSecrets\<USER_SECRETS_ID>\secrets.json

type $env:APPDATA\Microsoft\UserSecrets\<USER_SECRETS_ID>\secrets.json
```

```powershell
# Linux / macOS

cat ~/.microsoft/usersecrets/<USER_SECRETS_ID>/secrets.json
```

```bash
# Windows CMD

type %APPDATA%\Microsoft\UserSecrets\<USER_SECRETS_ID>\secrets.json
```

> **NOTE**
>
> Substitute &lt;USER\__SECRETS_\_ID&gt; with the GUID found in the project file.

```xml
<UserSecretsId>567f0dg2-06b1-5567-94a5-8c855a1a7972</UserSecretsId>
```

#### Step 2 - Add User Secrets

We need to add 2 user secrets representing the 2 values required by the AWS SDK:

* Profile
* Region

In the section [Using appsettings](#using-appsettings), we had the following configuration:

```javascript
{
    "AWS": {
        "Region": "us-east-1",
        "Profile": "aws-demo"
    }
}
```

We now need to add 2 user secrets that align with the aforementioned AWS configuration:

```powershell
dotnet user-secrets set "AWS:Profile" "aws-demo"
dotnet user-secrets set "AWS:Region" "us-east-1"
```

#### Step 3 - List User Secrets

```powershell
dotnet user-secrets list
```

#### Step 4 - Remove and Clear User Secrets

To individually remove user secrets, use the following commands:

```powershell
dotnet user-secrets remove "AWS:Profile"
dotnet user-secrets remove "AWS:Region"
```

Alternatively, you can clear all user secrets using the following command:

```powershell
dotnet user-secrets clear
```

---

## Signup With AWS Free Tier

> **WARNING**
>
> You need a valid credit card to signup for the AWS Free Tier

AWS offers a generous free tier with a relatively straight forward signup process. Unfortunately, you will need a credit card to complete the signup process. Go to [Signup](https://portal.aws.amazon.com/billing/signup).

> **NOTE**
>
> Check out [this official getting started video](https://youtu.be/v3WLJ_0hnOU) \(5 mins\) that takes you through the signup process

For more information about the [AWS Free Tier](https://aws.amazon.com/free), see the following links:

* [AWS Pricing](https://aws.amazon.com/pricing)
* [AWS Fee Tier](https://aws.amazon.com/free)
* [12 Months Free](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=tier%2312monthsfree&awsf.Free%20Tier%20Categories=categories%23analytics%7Ccategories%23app-integration%7Ccategories%23compute%7Ccategories%23databases%7Ccategories%23devtools%7Ccategories%23mobile%7Ccategories%23serverless%7Ccategories%23ai-ml)
* [Always Free](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=tier%23always-free&awsf.Free%20Tier%20Categories=categories%23analytics%7Ccategories%23app-integration%7Ccategories%23compute%7Ccategories%23databases%7Ccategories%23devtools%7Ccategories%23mobile%7Ccategories%23serverless%7Ccategories%23ai-ml)
* [Signup](https://portal.aws.amazon.com/billing/signup)

---

## Learning

AWS is vast and there are many great resources to learn about AWS. The following list is a curated list of learning resources that can be used to learn and remain current with AWS technology.

### Official AWS Resources

* [AWS Learning Library](https://www.aws.training/LearningLibrary)
* [AWS Cloud Practitioner Essentials](https://www.aws.training/Details/eLearning?id=60697)
* [AWS Whitepapers and Guides](https://aws.amazon.com/whitepapers/?whitepapers-main.sort-by=item.additionalFields.sortDate&whitepapers-main.sort-order=desc)
* [AWS Blogs](https://aws.amazon.com/blogs/aws)
* [AWS Podcasts](https://aws.amazon.com/podcasts/aws-podcast)
* [AWS Twitter](https://twitter.com/awscloud)
* [AWS Twitch](https://www.twitch.tv/aws)

---

## Certification

### AWS

* [Exam Readiness: AWS Certified Developer Associate](https://www.aws.training/Details/Curriculum?id=19185)
* [Exam Readiness: AWS Certified Architect Associate](https://www.aws.training/Details/Curriculum?id=20685)
* [Exam Readiness: AWS Certified Sysops Associate](https://www.aws.training/Details/Video?id=27486)

### Udemy

* [AWS Certified Cloud Practitioner](https://www.udemy.com/course/aws-certified-cloud-practitioner-new)
* [AWS Certified Developer Associate](https://www.udemy.com/course/aws-certified-developer-associate-dva-c01/)
* [AWS Certified Solutions Architect Associate](https://www.udemy.com/course/aws-certified-solutions-architect-associate-saa-c02/)
* [AWS Certified Sysops Administrator Associate](https://www.udemy.com/course/ultimate-aws-certified-sysops-administrator-associate)

### ACloudGuru

* [AWS Cloud Training](https://acloudguru.com/aws-cloud-training)
* [AWS Certified Cloud Practitioner](https://acloudguru.com/course/aws-certified-cloud-practitioner)
* [AWS Certified Developer Associate](https://acloudguru.com/course/aws-certified-developer-associate)
* [AWS Certified Architect Associate](https://acloudguru.com/course/aws-certified-solutions-architect-associate-saa-c02)
* [AWS Certified Sysops Administrator Associate](https://acloudguru.com/course/aws-certified-sysops-administrator-associate)

### Whizlabs

* [Whizlabs AWS Cloud Training](https://www.whizlabs.com/cloud-certification-training-courses)
* [AWS Certified Cloud Practitioner](https://www.whizlabs.com/aws-certified-cloud-practitioner)
* [AWS Certified Developer Associate](https://www.whizlabs.com/aws-developer-associate)
* [AWS Certified Architect Associate](https://www.whizlabs.com/aws-solutions-architect-associate/)
* [AWS Certified Sysops Administrator Associate](https://www.whizlabs.com/aws-sysops-administrator-associate)

---

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/drminnaar/aws-dotnetcore-examples/tags).

---

## Authors

* **Douglas Minnaar** - *Initial work* - [drminnaar](https://github.com/drminnaar)

---

[Cognito Mvc]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/cognito-mvc-web
[Cognito Api]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/cognito-mvc-api
[S3 ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/s3-console
[DynamoDb ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/dynamodb-console
[SNS ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/sns-console
[SQS ConsoleApp]: https://github.com/drminnaar/aws-dotnetcore-examples/tree/master/sqs-console
[Creating .NET Core AWS Lambda Projects without Visual Studio]: https://aws.amazon.com/blogs/developer/creating-net-core-aws-lambda-projects-without-visual-studio/
[.NET5]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 20.04]: https://releases.ubuntu.com/focal/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon S3]: https://docs.aws.amazon.com/s3
[Amazon SNS]: https://docs.aws.amazon.com/sns/latest/dg/welcome.html
[Amazon SQS]: https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/welcome.html
[Amazon Cognito]: https://aws.amazon.com/cognito/getting-started/
[Amazon DynamoDb]: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Introduction.html
