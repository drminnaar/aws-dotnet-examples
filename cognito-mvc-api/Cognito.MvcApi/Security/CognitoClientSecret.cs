using System;
using System.Security.Cryptography;
using System.Text;
using Amazon.AspNetCore.Identity.Cognito.Extensions;

namespace Cognito.MvcApi.Security
{
    public sealed class CognitoClientSecret
    {
        private readonly AWSCognitoClientOptions _options;

        public CognitoClientSecret(AWSCognitoClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string ComputeHash(string username)
        {
            var dataString = username + _options.UserPoolClientId;

            var data = Encoding.UTF8.GetBytes(dataString);
            var key = Encoding.UTF8.GetBytes(_options.UserPoolClientSecret);

            return Convert.ToBase64String(HmacSHA256(data, key));
        }

        private static byte[] HmacSHA256(byte[] data, byte[] key)
        {
            using var shaAlgorithm = new HMACSHA256(key);
            return shaAlgorithm.ComputeHash(data);
        }        
    }
}