![aws-cognito-dotnet-github](https://user-images.githubusercontent.com/33935506/114387401-7dc86300-9be6-11eb-8ab6-f8d843de20cc.png)

# AWS Cognito API Example

This project demonstrates how to integrate a .NET Core Web Api application with Amazon Cognito using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core Web Api application
  * Setup AWS services
  * Setup Authentication using JWT Bearer
  * Setup CORS
* Learn what the Amazon SDK Nuget packages are required to integrate with Amazon Cognito

## Feature Overview

This Web Api application provides the following endpoints:

* Signups
  * POST api/signups - Allows an api consumer to signup for a new account
* Tokens
  * POST api/tokens - Provides JWT access token for valid account credentials
* Values
  * GET api/values - An unauthorized endpoint that returns a list of values
  * GET api/values/123 - An authorized endpoint (Requires JWT access token) that returns a single value

---

## Getting Started

### Configure User Secrets

```powershell
# change directory to Cognito.MvcApi
cd ./Cognito.MvcApi

# initialize user secrets
dotnet user-secrets init

# add AWS configuration options to user secrets
dotnet user-secrets set "AWS:Profile" "<YOUR_CHOSEN_PROFILE_NAME>"
dotnet user-secrets set "AWS:Region" "<YOUR_CHOSEN_REGION_NAME>"
dotnet user-secrets set "AWS:UserPoolClientId" "<YOUR_POOL_CLIENT_ID>"
dotnet user-secrets set "AWS:UserPoolClientSecret" "<YOUR_POOL_SECRET>"
dotnet user-secrets set "AWS:UserPoolId" "<YOUR_POOL_ID>"

# verify that secrets were added correctly and successfully
dotnet user-secrets list

AWS:Region = <YOUR_CHOSEN_REGION_NAME>
AWS:Profile = <YOUR_CHOSEN_PROFILE_NAME>
AWS:UserPoolId = <YOUR_POOL_ID>
AWS:UserPoolClientSecret = <YOUR_POOL_SECRET>
AWS:UserPoolClientId = <YOUR_POOL_CLIENT_ID>

# clear secrets if you no longer need them
dotnet user-secrets clear
```

### Start Application

```powershell
# change directory to Cognito.MvcApi
cd ./Cognito.MvcApi

# run app
dotnet run
```

---

## Test Api

[Rest Client] allows you to send HTTP request and view the response in [Visual Studio Code] directly.

Requests for each of the Web Api endpoints can be found in the **[Requests](https://github.com/drminnaar/aws-dotnet-examples/tree/master/cognito-mvc-api/Requests)** folder

* [Requests/Signups.http](https://github.com/drminnaar/aws-dotnet-examples/blob/master/cognito-mvc-api/Requests/Signups.http)

  ```json
  ### Create signup using valid details

  POST http://localhost:5000/api/signups
  User-Agent: rest-client
  Content-Type: application/json

  {
      "Email": "bob@example.com",
      "Password": "password",
      "ConfirmedPassword": "password"
  }
  ```

* [Requests/Tokens.http](https://github.com/drminnaar/aws-dotnet-examples/blob/master/cognito-mvc-api/Requests/Tokens.http)

  ```json
  ### Get Token using valid credentials

  POST http://localhost:5000/api/tokens
  User-Agent: rest-client
  Content-Type: application/json

  {
      "Email": "bob@example.com",
      "Password": "password"
  }
  ```

* [Requests/Values.http](https://github.com/drminnaar/aws-dotnet-examples/blob/master/cognito-mvc-api/Requests/Values.http)

  ```bash
  ### Get 'Values' from unauthorized endpoint

  GET http://localhost:5000/api/values

  ### Get 'Value' from authorized endpoint without token

  GET http://localhost:5000/api/values/178

  ### Get 'Value' from authorized endpoint with token

  GET http://localhost:5000/api/values/178
  Authorization: Bearer <<enter_your_token_here>>
  ```

---

## Notable Nuget Packages

* [Amazon.AspNetCore.Identity.Cognito] - Simplifies using Amazon Cognito as a membership storage solution for building ASP.NET Core web applications using ASP.NET Core Identity.

* [Amazon.Extensions.CognitoAuthentication] - An extension library to assist in the Amazon Cognito User Pools authentication process.

---

[.NET Core]: https://www.microsoft.com/net/download
[C#]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
[Ubuntu 18.04]: http://releases.ubuntu.com/bionic/
[Visual Studio Code]: https://code.visualstudio.com/
[Amazon Cognito]: https://aws.amazon.com/cognito/getting-started/
[Amazon.AspNetCore.Identity.Cognito]: https://www.nuget.org/packages/Amazon.AspNetCore.Identity.Cognito/
[Amazon.Extensions.CognitoAuthentication]: https://www.nuget.org/packages/Amazon.Extensions.CognitoAuthentication/
[Rest Client]: https://marketplace.visualstudio.com/items?itemName=humao.rest-client