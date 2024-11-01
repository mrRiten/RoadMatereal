using Microsoft.AspNetCore.Identity;

namespace RoadMatereal.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        // Additional properties for user profile information
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        // Optional middle name field
        public string? MiddleName { get; set; }

        // Navigation property for roles associated with this user (if needed)
        // This is optional as roles are managed through the Role entity.
        //public ICollection<Role>? Roles {get ;set;}
    }
}