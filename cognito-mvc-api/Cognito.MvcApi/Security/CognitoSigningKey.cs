using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Cognito.MvcApi.Security
{
    public sealed class CognitoSigningKey
    {
        private readonly string _userPoolClientSecret;

        public CognitoSigningKey(string userPoolClientSecret)
        {
            _userPoolClientSecret = userPoolClientSecret;
        }

        public SymmetricSecurityKey ComputeKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_userPoolClientSecret));
        }
    }
}