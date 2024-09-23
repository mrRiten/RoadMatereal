using Microsoft.AspNetCore.Identity;

namespace RoadMatereal.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
