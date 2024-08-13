using Project.Models;
using System.Collections.Generic;

namespace Project.ViewModels
{
    public class DashboardViewModel
    {
        public User User { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public EducationalDetails EducationalDetails { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public List<Campus> RecommendedCampuses { get; set; }
        // Add other properties for different recommendations as needed.
    }
}
