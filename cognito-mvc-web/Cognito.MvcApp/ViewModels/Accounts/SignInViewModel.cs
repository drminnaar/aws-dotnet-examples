using System.ComponentModel.DataAnnotations;

namespace Cognito.MvcApp.ViewModels.Accounts
{
    public sealed class SignInViewModel
    {
        [Required(ErrorMessage = "An email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}