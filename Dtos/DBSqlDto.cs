using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class DBSqlDto
    {
        public string RawSQL { get; set; }
        public DataTable RawList { get; set; }
        public int? NoOfRowsAffected { get; set; }
    }
}