# Cognito MVC README

This project demonstrates how to integrate an [ASP.NET Core] MVC web application with AWS Cognito using the Amazon SDK. The [ASP.NET Core] MVC web application uses [Amazon Cognito] as an identity provider.

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
# change directory to Cognito.MvcApp
cd ./Cognito.MvcApp

# run app
dotnet run
```

### Signup User

Provide valid Name, Email and Password to signup.

![cognito-signup-1](https://user-images.githubusercontent.com/33935506/114539965-1e7f5700-9ca9-11eb-90a8-cbfe4e4f560d.png)

### Verify Email

![cognito-signup-2](https://user-images.githubusercontent.com/33935506/114539971-1fb08400-9ca9-11eb-99f0-0da931b14875.png)

### Signin

![cognito-signup-3](https://user-images.githubusercontent.com/33935506/114539975-20491a80-9ca9-11eb-8b68-95c45f879519.png)

---

## Feature Overview

The following features have been implemented:

* Signup for a new account
* Confirm Signup (using confirmation code)
* Sign into account
* Sign out of account

---

## Feature Walkthrough

### Signup for a new account

```csharp
// File: SignupsController.cs

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Signup(SignupViewModel signup)
{
    if (!ModelState.IsValid)
        return View(signup);

    var cognitoUser = _pool.GetUser(signup.Email);

    if (cognitoUser == null)
        throw new InvalidOperationException("Expected a non-null Cognito user to be returned.");

    if (!string.IsNullOrWhiteSpace(cognitoUser.Status))
    {
        ModelState.AddModelError(string.Empty, $"A user having email '{signup.Email}' already exists.");
        return View(signup);
    }

    cognitoUser.Attributes.Add(CognitoAttribute.Name.AttributeName, signup.Name);
    cognitoUser.Attributes.Add(CognitoAttribute.Email.AttributeName, signup.Email);

    var identityResult = await _userManager.CreateAsync(cognitoUser, signup.Password);

    if (!identityResult.Succeeded)
    {
        ModelState.AddModelError(string.Empty, $"A user having email '{signup.Email}' could not be created.");

        foreach (var error in identityResult.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(signup);
    }

    return RedirectToAction(nameof(Confirm), new { email = signup.Email });
}
```

### Confirm Signup (using confirmation code)

```csharp
// File: SignupsController.cs

[HttpGet]
public IActionResult Confirm(string email)
{
    return View(new SignupConfirmationViewModel { Email = email });
}

[HttpPost]
public async Task<IActionResult> Confirm(SignupConfirmationViewModel confirmation)
{
    if (!ModelState.IsValid)
        return View(confirmation);

    var user = await _userManager.FindByEmailAsync(confirmation.Email);

    if (user == null)
    {
        ModelState.AddModelError(string.Empty, $"A user having email '{confirmation.Email}' does not exist.");
        return View(confirmation);
    }

    var confirmationResult = await ((CognitoUserManager<CognitoUser>)_userManager)
        .ConfirmSignUpAsync(user, confirmation.Code, true);

    if (!confirmationResult.Succeeded)
    {
        foreach (var error in confirmationResult.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(confirmation);
    }

    return RedirectToAction(nameof(Complete));
}
```

### Sign into account

```csharp
// File: AccountsController.cs

[HttpGet]
public IActionResult SignIn()
{
    return View(new SignInViewModel());
}

[HttpPost]
public async Task<IActionResult> SignIn(SignInViewModel signIn)
{
    if (!ModelState.IsValid)
        return View(signIn);

    var user = await _userManager.FindByEmailAsync(signIn.Email);

    if (user == null)
    {
        ModelState.AddModelError(string.Empty, $"A user having email '{signIn.Email}' does not exist.");
        return View(signIn);
    }

    var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);

    if (!isConfirmed)
    {
        ModelState.AddModelError(string.Empty, $"Your account having email '{signIn.Email}' has not yet been confirmed.");
        return View(signIn);
    }

    var signInResult = await _signInManager.PasswordSignInAsync(
        user,
        signIn.Password,
        isPersistent: false,
        lockoutOnFailure: false);

    if (!signInResult.Succeeded)
    {
        ModelState.AddModelError(string.Empty, "Invalid credentials provided");
        return View(signIn);
    }

    return RedirectToAction(nameof(DashboardController.Index), DashboardController.Name);
}
```

### Sign out of account

```csharp
// File: AccountsController.cs

[Authorize]
public async Task<IActionResult> LogOut()
{
    if (User.Identity?.IsAuthenticated ?? false)
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(SignIn));
    }

    return RedirectToAction(nameof(DashboardController.Index), DashboardController.Name);
}
```

---

## Notable Nuget Packages

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