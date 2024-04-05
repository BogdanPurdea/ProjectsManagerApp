using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public AppUser Creator { get; set; }
        public bool IsFinished { get; set; }
        public bool IsApproved { get; set; }
        public ICollection<AppUser>? Contributors { get; set; }
    }
}