# Cognito MVC README

This project demonstrates how to integrate an [ASP.NET Core] MVC web application with AWS Cognito using the Amazon SDK. The [ASP.NET Core] MVC web application uses [Amazon Cognito] as an identity provider.

## Features

The following features have been implemented:

* Signup for a new account
* Confirm Signup (using confirmation code)
* Sign into account
* Sign out of account

## Developed With

The entire application was built using [Visual Studio Code] on [Ubuntu 18.04]. The following list is a summary of the primary tools, languages and frameworks used to build the application:

* [Visual Studio Code] - Visual Studio Code is a source-code editor developed by Microsoft for Windows, Linux and macOS.

* [Ubuntu 18.04] - Ubuntu is an open source software operating system that runs from the desktop, to the cloud, to all your internet connected things.

* [Amazon Cognito] - An AWS service that provides authentication, authorization, and user management for your web and mobile apps.

* [.NET Core] - .NET Core is a free and open-source managed software framework for Linux, Windows and macOS.

* [C#] - A multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.

* [ASP.NET Core] - ASP.NET Core is a free, cross-platform, and open-source web framework

* [ASP.NET Core Identity] - ASP.NET Core Identity is a membership system that adds login functionality to ASP.NET Core apps.

* [Bootstrap 4] - Build responsive, mobile-first projects

### Notable Nuget Packages

* [OdeToCode.UseNodeModules] - ASP.NET Core middleware to serve files from the node_modules directory in the root of the project.

* [Amazon.AspNetCore.Identity.Cognito] - Simplifies using Amazon Cognito as a membership storage solution for building ASP.NET Core web applications using ASP.NET Core Identity.

* [Amazon.Extensions.CognitoAuthentication] - An extension library to assist in the Amazon Cognito User Pools authentication process.


[Bootstrap 4]: https://getbootstrap.com
[ASP.NET Core]: https://www.asp.net/
[ASP.NET Core 2.1]: https://www.asp.net/
[ASP.NET Core Identity]: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity
[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[OdeToCode.UseNodeModules]: https://github.com/OdeToCode/UseNodeModules
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon Cognito]: https://docs.aws.amazon.com/cognito/latest/developerguide/what-is-amazon-cognito.html
[Cognito]: https://docs.aws.amazon.com/cognito/latest/developerguide/what-is-amazon-cognito.html
[Cognito User Pools]: https://docs.aws.amazon.com/cognito/latest/developerguide/cognito-user-identity-pools.html
[Amazon.AspNetCore.Identity.Cognito]: https://www.nuget.org/packages/Amazon.AspNetCore.Identity.Cognito/
[Amazon.Extensions.CognitoAuthentication]: https://www.nuget.org/packages/Amazon.Extensions.CognitoAuthentication/