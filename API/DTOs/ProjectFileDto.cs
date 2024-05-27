namespace API.DTOs
{
    public class ProjectFileDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
    }
}