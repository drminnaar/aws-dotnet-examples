using System;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito.Extensions;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Cognito.MvcApi.Models.Signups;
using Cognito.MvcApi.Security;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SignupsController : ControllerBase
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly AWSCognitoClientOptions _options;
        private readonly CognitoClientSecret _clientSecret;

        public SignupsController(
            IAmazonCognitoIdentityProvider identityProvider,
            AWSCognitoClientOptions options,
            CognitoClientSecret clientSecret)
        {
            _identityProvider = identityProvider ?? throw new ArgumentNullException(nameof(identityProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        }

        [HttpPost]
        public async Task<IActionResult> SignupAsync(Signup signup)
        {
            var request = new SignUpRequest
            {
                ClientId = _options.UserPoolClientId,
                Password = signup.Password,
                SecretHash = _clientSecret.ComputeHash(signup.Email),
                Username = signup.Email
            };

            try
            {
                await _identityProvider.SignUpAsync(request);
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
    }
}