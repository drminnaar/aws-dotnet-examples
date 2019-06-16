# DynamoDb Console Application README

This project demonstrates how to integrate a .NET Core console application with DynamoDb using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with DynamoDB

## Features

This application provides the functionality required to both manage DynamoDb tables, and manage the data stored in a DynamoDB table

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

## Developed With

The entire application was built using [Visual Studio Code] on [Ubuntu 18.04]. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.

* [Ubuntu 18.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

* [Amazon DynamoDb] - Amazon DynamoDB is a fully managed NoSQL database service that provides fast and predictable performance with seamless scalability.

* [.NET Core] - .NET Core is a free and open-source managed software framework for Linux, Windows and macOS.

* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

### Notable Nuget Packages

* [AWSSDK.DynamoDBv2] - Amazon DynamoDB is a fast and flexible NoSQL database service for all applications that need consistent, single-digit millisecond latency at any scale.

* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon DynamoDb]: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Introduction.html
[AWSSDK.DynamoDBv2]: https://www.nuget.org/packages/AWSSDK.DynamoDBv2/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/