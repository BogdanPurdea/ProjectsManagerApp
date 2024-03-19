using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? CreatorName { get; set; }
        public bool IsFinished { get; set; }
        public bool IsApproved { get; set; }
        public ICollection<string>? Contributors { get; set; }
    }
}