using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Account
{
    public class ForgotPasswordViewmodel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
