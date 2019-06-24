# AWS Cognito API Example

This project demonstrates how to integrate a .NET Core Web Api application with Amazon Cognito using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core Web Api application
  * Setup AWS services
  * Setup Authentication using JWT Bearer
  * Setup CORS
* Learn what the Amazon SDK Nuget packages are required to integrate with Amazon Cognito

## Features

This Web Api application provides the following endpoints:

* Signups
  * POST api/signups - Allows an api consumer to signup for a new account
* Tokens
  * POST api/tokens - Provides JWT access token for valid account credentials
* Values
  * GET api/values - An unauthorized endpoint that returns a list of values
  * GET api/values/123 - An authorized endpoint (Requires JWT access token) that returns a single value

## Developed With

The entire application was built using [Visual Studio Code] on [Ubuntu 18.04]. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.

* [Ubuntu 18.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

* [Amazon Cognito] - Amazon Simple Notification Service (Amazon SNS) is a web service that coordinates and manages the delivery or sending of messages to subscribing endpoints or clients.

* [.NET Core] - .NET Core is a free and open-source managed software framework for Linux, Windows and macOS.

* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

### Notable Nuget Packages

* [Amazon.AspNetCore.Identity.Cognito] - Simplifies using Amazon Cognito as a membership storage solution for building ASP.NET Core web applications using ASP.NET Core Identity.

* [Amazon.Extensions.CognitoAuthentication] - An extension library to assist in the Amazon Cognito User Pools authentication process.

## Test Api

[Rest Client] allows you to send HTTP request and view the response in [Visual Studio Code] directly.

Requests for each of the Web Api endpoints can be found in the _'Requests'_ folder

* Requests/Signups.http
* Requests/Tokens.http
* Requests/Values.http

[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon Cognito]: https://aws.amazon.com/cognito/getting-started/
[Amazon.AspNetCore.Identity.Cognito]: https://www.nuget.org/packages/Amazon.AspNetCore.Identity.Cognito/
[Amazon.Extensions.CognitoAuthentication]: https://www.nuget.org/packages/Amazon.Extensions.CognitoAuthentication/
[Rest Client]: https://marketplace.visualstudio.com/items?itemName=humao.rest-client