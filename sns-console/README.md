# SNS (Simple Noticiation Service) Console Application README

This project demonstrates how to integrate a .NET Core console application with SNS using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with SNS

## Features

This application provides the functionality required to both manage SNS topcs, subscriptions, and publications.

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

## Developed With

The entire application was built using [Visual Studio Code] on [Ubuntu 18.04]. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.

* [Ubuntu 18.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

* [Amazon SNS] - Amazon Simple Notification Service (Amazon SNS) is a web service that coordinates and manages the delivery or sending of messages to subscribing endpoints or clients.

* [.NET Core] - .NET Core is a free and open-source managed software framework for Linux, Windows and macOS.

* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

### Notable Nuget Packages

* [AWSSDK.SimpleNotificationService] - Amazon Simple Notification Service (Amazon SNS) is a fast, flexible, fully managed push messaging service.

* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon SNS]: https://docs.aws.amazon.com/sns/latest/dg/welcome.html
[AWSSDK.SimpleNotificationService]: https://www.nuget.org/packages/AWSSDK.SimpleNotificationService/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/