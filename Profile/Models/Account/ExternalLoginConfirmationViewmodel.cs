using Profile.Tenant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Account
{
    public class ExternalLoginConfirmationViewmodel
    {
        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public List<ValueField> Required { get; set; }
        public List<ValueField> Optional { get; set; }

        public string ReturnUrl { get; set; }
    }
}
