// ViewModels/UserProfileViewModel.cs
using RoadMatereal.Models;
using System.Collections.Generic;

namespace RoadMatereal.ViewModels
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }

        // Initialize Orders to avoid null reference issues
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}