using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Models
{
    //[Table("Forms", Schema = "novaweb")]
    [Table("Forms")]
    public class Forms
    {
        [Key]
        public Int64 FormsID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string Phone { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public decimal LoanAmount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceState { get; set; }
        public string IP { get; set; }
        public bool FormIsSubmit { get; set; }
        public int FormNo { get; set; }
        public string FormImagePath { get; set; }
        public string FormsCreatedByUserID { get; set; }
        public DateTime FormsCreatedDate { get; set; }
        public string FormsUpdatedByUserID { get; set; }
        public DateTime? FormsUpdatedDate { get; set; }
    }
}