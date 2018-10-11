using Profile.Tenant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Manage
{
    public class SaveLoginModel
    {
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Beide wachtwoorden dienen hetzelfde te zijn.")]
        public string ConfirmPassword { get; set; }
    }

    public class SaveProfileModel
    {
        public List<ValueField> Required { get; set; }
        public List<ValueField> Optional { get; set; }
    }

    public class ExternalLoginModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
