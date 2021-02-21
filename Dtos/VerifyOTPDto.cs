using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class VerifyOTPDto
    {
        [Required]
        [Display(Name = "Verify OTP")]
        public int OTP { get; set; }
    }
}