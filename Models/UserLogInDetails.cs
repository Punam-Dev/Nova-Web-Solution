using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Models
{
    //[Table("UserLogInDetails", Schema = "novaweb")]
    [Table("UserLogInDetails")]
    public class UserLogInDetails
    {
        public Int64 UserLogInDetailsID { get; set; }
        public string UserID { get; set; }
        public string UserIP { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsLogIn { get; set; }
    }
}