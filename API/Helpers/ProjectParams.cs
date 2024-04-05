using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class ProjectParams : PaginationParams
    {
        public string? ProjectName { get; set; }

        public string OrderBy { get; set; } = "id";

        public bool hasUser { get; set; } = false;
    }
}