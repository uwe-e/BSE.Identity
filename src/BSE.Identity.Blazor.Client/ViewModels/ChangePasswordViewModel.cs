using Elfie.Serialization;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BSE.Identity.Blazor.Client.ViewModels
{
    public class ChangePasswordViewModel
    {
        private readonly IStringLocalizer<ChangePasswordViewModel> _stringLocalizer;

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public ChangePasswordViewModel(IStringLocalizer<ChangePasswordViewModel> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
    }
}
