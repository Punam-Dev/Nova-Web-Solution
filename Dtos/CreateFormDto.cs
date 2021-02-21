using NovaWebSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Dtos
{
    public class CreateFormDto
    {
        public Forms Forms { get; set; }
        public List<FormQueryDto> formQueryDtos { get; set; }
    }
}