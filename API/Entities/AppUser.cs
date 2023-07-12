using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<Project>? CreatedProjects { get; set; }
        public ICollection<Project>? AssociatedProjects { get; set; }
        public ICollection<Photo>? Photos { get; set; }
        public ICollection<Message>? MessagesSent { get; set; }
        public ICollection<Message>? MessagesReceived { get; set; }
        public ICollection<AppUserRole>? UserRoles { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }
    }
}