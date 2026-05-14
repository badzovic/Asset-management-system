using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}