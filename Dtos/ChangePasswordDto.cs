using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class ChangePasswordDto
    {
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password must match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}