using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace yalms.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Mejladress")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Kom ihåg denna webbläsare")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Mejladress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Användarnamn")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Förnamn Efternamn")]
        [StringLength(100, ErrorMessage = "{0} måste bestå av minst {2} tecken.", MinimumLength = 10)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Telefon")]
        [StringLength(100, ErrorMessage = "{0} måste bestå av minst {2} tecken.", MinimumLength = 8)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Ditt {0} måste bestå av minst {2} tecken.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Verifiera lösenord")]
        [Compare("Password", ErrorMessage = "De angivna lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Mejladress")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Ditt {0} måste bestå av minst {2} tecken.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Verifiera lösenord")]
        [Compare("Password", ErrorMessage = "De angivna lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Mejladress")]
        public string Email { get; set; }
    }

    public class UserRoleModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
