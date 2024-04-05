using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams : PaginationParams
    { 
        public string? CurrentUserName { get; set; }

        public string OrderBy { get; set; } = "lastActive";
    }
}