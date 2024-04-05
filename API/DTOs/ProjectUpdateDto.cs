using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProjectUpdateDto
    {
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
    }
}