using System.ComponentModel.DataAnnotations;

namespace Cognito.MvcApp.ViewModels.Signups
{
    public sealed class SignupViewModel
    {
        [Required(ErrorMessage = "An email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmation password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Password")]
        public string PasswordConfirmation { get; set; } = string.Empty;

        [Required(ErrorMessage = "A name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;
    }
}
