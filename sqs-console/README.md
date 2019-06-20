# SQS (Simple Queue Service) Console Application README

This project demonstrates how to integrate a .NET Core console application with SQS using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with SQS

## Features

This application provides the functionality required to manage SQS queues.

The following features have been implemented:

* Manage Queues
  * Create queue
  * List queues
  * Delete queue
  * Get queue url
* Manage Game Ranking Queue
  * Enqueue game ranking to queue
  * Dequeue game rankings from queue

## Developed With

The entire application was built using [Visual Studio Code] on [Ubuntu 18.04]. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.

* [Ubuntu 18.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

* [Amazon SQS] - Amazon Simple Queue Service (SQS) is a fully managed message queuing service that enables you to decouple and scale microservices, distributed systems, and serverless applications.

* [.NET Core] - .NET Core is a free and open-source managed software framework for Linux, Windows and macOS.

* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

### Notable Nuget Packages

* [AWSSDK.SQS] - Amazon Simple Notification Service (Amazon SQS) is a fast, flexible, fully managed push messaging service.

* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon SQS]: https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/welcome.html
[AWSSDK.SQS]: https://www.nuget.org/packages/AWSSDK.SQS/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/