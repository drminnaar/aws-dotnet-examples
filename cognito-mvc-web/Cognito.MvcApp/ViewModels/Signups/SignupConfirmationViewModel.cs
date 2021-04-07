using System;

namespace Cognito.MvcApp.ViewModels.Signups
{
    public sealed class SignupConfirmationViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
