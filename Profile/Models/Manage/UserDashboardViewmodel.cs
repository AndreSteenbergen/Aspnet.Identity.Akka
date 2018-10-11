using Profile.Tenant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Manage
{
    public class UserDashboardViewmodel
    {
        public Dictionary<SocialLogin, string> SocialLogins { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Beide wachtwoorden dienen hetzelfde te zijn.")]
        public string ConfirmPassword { get; set; }
        public bool HasPassword { get; set; }

        public List<ValueField> Required { get; set; }
        public List<ValueField> Optional { get; set; }
    }
}
