using Profile.Tenant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Account
{
    public class RegisterViewmodel
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Beide wachtwoorden dienen hetzelfde te zijn.")]
        public string ConfirmPassword { get; set; }

        public List<ValueField> Required { get; set; }
        public List<ValueField> Optional { get; set; }

        public string ReturnUrl { get; set; }
    }
}
