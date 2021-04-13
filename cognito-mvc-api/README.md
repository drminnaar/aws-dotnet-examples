![aws-cognito-dotnet-github](https://user-images.githubusercontent.com/33935506/114387401-7dc86300-9be6-11eb-8ab6-f8d843de20cc.png)

# AWS Cognito API Example

This project demonstrates how to integrate a .NET Core Web Api application with Amazon Cognito using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core Web Api application
  * Setup AWS services
  * Setup Authentication using JWT Bearer
  * Setup CORS
* Learn what the Amazon SDK Nuget packages are required to integrate with Amazon Cognito

Contents:

* [Getting Started](#getting-started)
  * [Configure Cognito User Pool](#configure-cognito-user-pool)
  * [Configure User Secrets](#configure-user-secrets)
  * [Start Application](#start-application)
  * [Signup User](#signup-user)
* [Feature Overview](#feature-overview)
* [Feature Walkthrough](#feature-walkthrough)
* [Notable Nuget Packages](#notable-nuget-packages)

---

## Getting Started

### Configure Cognito User Pool

#### Step 1: Open Cognito

Open AWS Cognito and select **Manage User Pools**

![cognito-1](https://user-images.githubusercontent.com/33935506/114525038-dc9ae480-9c99-11eb-9e67-3a59f2d9067f.png)

#### Step 2: Create User Pool

Select **Create a user pool**

![cognito-2](https://user-images.githubusercontent.com/33935506/114525043-de64a800-9c99-11eb-9be0-46751514c2db.png)

Capture pool name and select **Review defaults**

![cognito-3](https://user-images.githubusercontent.com/33935506/114525045-de64a800-9c99-11eb-8a08-16d2c6220780.png)

Select **Create pool**

![cognito-4](https://user-images.githubusercontent.com/33935506/114525047-defd3e80-9c99-11eb-922e-8daec508e809.png)

![cognito-5](https://user-images.githubusercontent.com/33935506/114525049-defd3e80-9c99-11eb-8f56-f4dd2f0852cd.png)

![cognito-6](https://user-images.githubusercontent.com/33935506/114525051-df95d500-9c99-11eb-98f0-3242ea367359.png)

#### Step 3: Add App Client

Configure **Auth Flows Configuration**

![cognito-7](https://user-images.githubusercontent.com/33935506/114525053-df95d500-9c99-11eb-84a5-f1dfec71c520.png)

![cognito-9](https://user-images.githubusercontent.com/33935506/114525054-e02e6b80-9c99-11eb-9964-9e21dc1d2025.png)

#### Step 3: Configure App Client Settings

Configure **OAuth 2.0**

![cognito-10](https://user-images.githubusercontent.com/33935506/114525056-e02e6b80-9c99-11eb-99a3-9dc9828774f3.png)

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

### Signup User

#### Signup API Request

Execute signup request using Visual Studio Code [Rest Client].

[Requests/Signups.http](https://github.com/drminnaar/aws-dotnet-examples/blob/master/cognito-mvc-api/Requests/Signups.http)

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

#### Open "Users and Groups"

After signing up a user, open *Cognito User Pool* manager and view recently signed up users.

![cognito-11](https://user-images.githubusercontent.com/33935506/114525057-e0c70200-9c99-11eb-85b8-65a1ee9a4ebe.png)

#### Confirm User

Select the option to **Confirm User**

![cognito-12](https://user-images.githubusercontent.com/33935506/114525059-e0c70200-9c99-11eb-87f4-61a85fceb1e0.png)

#### User Confirmed

![cognito-13](https://user-images.githubusercontent.com/33935506/114525061-e15f9880-9c99-11eb-959d-733513674f32.png)

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

## Feature Overview

This Web Api application provides the following endpoints:

* [Configure App and AWS SDK](#configure-app-and-aws-sdk)
* [Signups](#signups)
  * POST api/signups - Allows an api consumer to signup for a new account
* [Tokens](#tokens)
  * POST api/tokens - Provides JWT access token for valid account credentials
* [Values](#values)
  * GET api/values - An unauthorized endpoint that returns a list of values
  * GET api/values/123 - An authorized endpoint (Requires JWT access token) that returns a single value

---

## Feature Walkthrough

### Configure App and AWS SDK

#### Configure AWS Cognito Identity

```csharp
// FILE: Startup.cs

// Setup AWS configuration and AWS Cognito Identity
var defaultOptions = _configuration.GetAWSOptions();
var cognitotOptions = _configuration.GetAWSCognitoClientOptions();
services.AddDefaultAWSOptions(defaultOptions);
services.AddSingleton(cognitotOptions);
services.AddSingleton(new CognitoClientSecret(cognitotOptions));
services.AddAWSService<IAmazonCognitoIdentityProvider>();
```

#### Configure Authentication

```csharp
// FILE: Startup.cs

// Setup authentication
services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authority = $"https://cognito-idp.us-east-1.amazonaws.com/{cognitotOptions.UserPoolId}";
        var audience = cognitotOptions.UserPoolClientId;

        options.Audience = audience;
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authority,
            ValidAudience = audience,
            IssuerSigningKey = new CognitoSigningKey(cognitotOptions.UserPoolClientSecret).ComputeKey()
        };
    });
```

#### Configure CORS

```csharp
// FILE: Startup.cs

// Setup CORS
services.AddCors(setup =>
{
    setup.AddPolicy("OpenSeason", policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowAnyHeader();
    });
});
```

#### Enable Middleware

```csharp
// FILE: Startup.cs

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseRouting();
    app.UseCors("OpenSeason");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

### Signups

```csharp
// FILE: SignupsController.cs

[HttpPost]
public async Task<IActionResult> SignupAsync(Signup signup)
{
    if (signup is null)
        throw new ArgumentNullException(nameof(signup));

    var request = new SignUpRequest
    {
        ClientId = _options.UserPoolClientId,
        Password = signup.Password,
        SecretHash = _clientSecret.ComputeHash(signup.Email),
        UserAttributes = new List<AttributeType>
        {
            new AttributeType { Name = "email", Value = signup.Email}
        },
        Username = signup.Email
    };

    try
    {
        await _identityProvider.SignUpAsync(request).ConfigureAwait(true);
    }
    catch (UsernameExistsException)
    {
        ModelState.AddModelError("UsernameExists", $"A user having the email '{signup.Email}' already exists.");
        return BadRequest(ModelState);
    }
    catch(InvalidParameterException e)
    {
        var key = e.Message.ToLower().Contains("username") ? "InvalidUsername" : "InvalidPassword";
        ModelState.AddModelError(key, e.Message);
        return BadRequest(ModelState);
    }
    catch(InvalidPasswordException e)
    {
        ModelState.AddModelError("InvalidPassword", e.Message);
        return BadRequest(ModelState);
    }

    return Ok();
}
```

### Tokens

```csharp
// FILE: TokensController.cs

[HttpPost]
public async Task<ActionResult<string>> GetAsync(TokenCredential credential)
{
    var request = new AdminInitiateAuthRequest
    {
        ClientId = _options.UserPoolClientId,
        UserPoolId = _options.UserPoolId,
        AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
    };

    // For ADMIN_NO_SRP_AUTH: USERNAME (required), SECRET_HASH (if app client is configured
    // with client secret), PASSWORD (required)
    request.AuthParameters.Add("USERNAME", credential.Email);
    request.AuthParameters.Add("PASSWORD", credential.Password);
    request.AuthParameters.Add("SECRET_HASH", _clientSecret.ComputeHash(credential.Email));

    string accessToken;

    try
    {
        var response = await _identityProvider.AdminInitiateAuthAsync(request);
        accessToken = response.AuthenticationResult.AccessToken;
    }
    catch (UserNotFoundException)
    {
        ModelState.AddModelError("UserNotFound", $"A user having email '{credential.Email}' does not exist.");
        return BadRequest(ModelState);
    }

    return accessToken;
}
```

### Values

```csharp
// FILE: ValuesController.cs

[HttpGet]
public ActionResult<IReadOnlyCollection<string>> Get()
{
    return new string[] { "value1", "value2" };
}

[HttpGet("{id}")]
[Authorize]
public ActionResult<string> Get(int id)
{
    return $"value{id}";
}
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