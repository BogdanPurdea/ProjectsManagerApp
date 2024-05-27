using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace API.Entities
{
    [Table("Files")]
    public class ProjectFile
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? PublicId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}