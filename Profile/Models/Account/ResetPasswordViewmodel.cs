﻿using System.ComponentModel.DataAnnotations;

namespace Profile.Models.Account
{
    public class ResetPasswordViewmodel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
