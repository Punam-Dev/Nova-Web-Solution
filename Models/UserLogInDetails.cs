using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Models
{
    public class UserLogInDetails
    {
        public Int64 UserLogInDetailsID { get; set; }
        public string UserID { get; set; }
        public string UserIP { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsLogIn { get; set; }
    }
}