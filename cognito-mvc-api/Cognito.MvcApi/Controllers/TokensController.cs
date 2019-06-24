using System;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito.Extensions;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Cognito.MvcApi.Models.Tokens;
using Cognito.MvcApi.Security;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TokensController : ControllerBase
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly AWSCognitoClientOptions _options;
        private readonly CognitoClientSecret _clientSecret;

        public TokensController(
            IAmazonCognitoIdentityProvider identityProvider,
            AWSCognitoClientOptions options,
            CognitoClientSecret clientSecret)
        {
            _identityProvider = identityProvider ?? throw new ArgumentNullException(nameof(identityProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        }

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

            string accessToken = string.Empty;

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

        
    }
}