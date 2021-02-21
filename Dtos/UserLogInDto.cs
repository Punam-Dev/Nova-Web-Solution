using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class UserLogInDto
    {
        [Required, Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}