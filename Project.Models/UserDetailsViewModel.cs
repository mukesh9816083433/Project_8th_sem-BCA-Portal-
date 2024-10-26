namespace Project.Models
{
    public class UserDetailsViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public EducationalDetails EducationalDetails { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public List<ApplicationViewModel>? ApplicationViewModel { get; set; }
        public ApplicationViewModel? ApplicationViewModel1 { get; set; }
    }
}