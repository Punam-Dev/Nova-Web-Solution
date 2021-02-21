using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class FormQueryDto
    {
        public FormQuery FormQuery { get; set; }
        public bool IsChecked { get; set; }
    }
}