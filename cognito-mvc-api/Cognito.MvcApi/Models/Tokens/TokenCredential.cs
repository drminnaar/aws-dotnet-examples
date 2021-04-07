using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.MvcApi.Models.Tokens
{
    [Serializable]
    public sealed class TokenCredential
    {
        [Required(ErrorMessage = "An email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}