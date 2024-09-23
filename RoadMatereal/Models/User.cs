using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace RoadMatereal.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
    }
}
