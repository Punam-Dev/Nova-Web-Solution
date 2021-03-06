using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Models
{
    //[Table("FormQuery", Schema = "novaweb")]
    [Table("FormQuery")]
    public class FormQuery
    {
        [Key]
        public Int64 FormQueryID { get; set; }
        public string FormQueryText { get; set; }
        public string FormQueryStatus { get; set; }
        public string FormQueryCreatedByUserID { get; set; }
        public DateTime FormQueryCreatedDate { get; set; }
        public string FormQueryUpdatedByUserID { get; set; }
        public DateTime? FormQueryUpdatedDate { get; set; }
    }
}