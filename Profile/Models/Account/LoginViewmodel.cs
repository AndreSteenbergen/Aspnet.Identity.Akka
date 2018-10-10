using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Account
{
    public class LoginViewmodel
    {
        public LoginViewmodel()
        {

        }

        public LoginViewmodel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public LoginViewmodel(LoginViewmodel model)
        {
            Username = model.Username;
            ReturnUrl = model.ReturnUrl;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
