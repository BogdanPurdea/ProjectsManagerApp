using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PhotoForApprovalDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string UserName { get; set; }
        public string IsApproved { get; set; }
    }
}