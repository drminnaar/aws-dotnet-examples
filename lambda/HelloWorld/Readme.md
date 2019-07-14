# AWS Lambda 'Hello World' Example

This is a very basic starter project and was generated using a combination of the [dotnet cli tool] and the __`lambda.EmptyFunction`__ template. For more information, please visit [Creating .NET Core AWS Lambda Projects without Visual Studio].

```powershell
dotnet new lambda.EmptyFunction -n HelloWorld
```

This starter project consists of:
* _Function.cs_ - class file containing a class with a single function handler method
* _aws-lambda-tools-defaults.json_ - default argument settings for use with Visual Studio and command line deployment tools for AWS

The generated function handler is a simple method accepting a string argument that returns the uppercase equivalent of the input string.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool] from the command line.

1. Install [Amazon.Lambda.Tools Global Tool] if not already installed.

```powershell
dotnet tool install -g Amazon.Lambda.Tools
```

2. If already installed check if new version is available.

```powershell
dotnet tool update -g Amazon.Lambda.Tools
```

3. Deploy function to AWS Lambda

```powershell
dotnet lambda deploy-function HelloWorld
```

4. Invoke function

```powershell
dotnet lambda invoke-function HelloWorld --payload "hello world'
```

## Explore Lambda Functions using AWS CLI

1. To list all functions:

```powershell
aws lambda list-functions
```

2. To get function:

```powershell
aws lambda get-function --function-name HelloWorld
```

3. To invoke function:

```powershell
aws lambda invoke \
--function-name HelloWorld \
--invocation-type RequestResponse \
--log-type Tail \
--payload '"hello world"' response.txt \
--query 'LogResult' \
--output text | base64 -d
```

4. To delete function

```powershell
aws lambda get-function --function-name HelloWorld
```

## Explore Lambda functions using AWS API

Please find a complete list of operations described by the [official AWS Lambda API documentation](https://docs.aws.amazon.com/lambda/latest/dg/API_Operations.html)

In order to test the Lambda API endpoints, a [Postman Collection] has been included in the root of this project called _'Lambda.postman_collection.json'_.

1. Import the collection into your [Postman] workspace.

2. Edit the collection 'Authorization' by specifying your own AWS _'access key'_ and _'secret key'_

![lambda-postman-collection-auth](https://user-images.githubusercontent.com/33935506/61182101-dae65c00-a682-11e9-8e77-df7db2cc7dcd.png)

3. Open any of the sample requests to test

   The following requests have been supplied as part of [Postman Collection]

   * List Functions

     ```http
     GET https://lambda.us-east-1.amazonaws.com/2015-03-31/functions
     ```
   * Get Function
     
     ```http
     GET https://lambda.us-east-1.amazonaws.com/2015-03-31/functions/HelloWorld
     ```
   * Invoke Function

     ```http
     POST https://lambda.us-east-1.amazonaws.com/2015-03-31/functions/HelloWorld/invocations
     ```
   * Delete Function

     ```http
     DELETE https://lambda.us-east-1.amazonaws.com/2015-03-31/functions/HelloWorld
     ```



[Creating .NET Core AWS Lambda Projects without Visual Studio]: https://aws.amazon.com/blogs/developer/creating-net-core-aws-lambda-projects-without-visual-studio/
[Amazon.Lambda.Tools Global Tool]: https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools
[dotnet cli tool]: https://docs.microsoft.com/en-us/dotnet/core/tutorials/using-with-xplat-cli
[Postman Collection]: https://www.getpostman.com/
[Postman]: https://www.getpostman.com/