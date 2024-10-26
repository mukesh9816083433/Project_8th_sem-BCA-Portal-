namespace Project.Models
{
    public class PersonalDetails
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String PhotoPath { get; set; }
        public int UserID { get; set; }

        public bool IsApproved { get; set; }
        public bool IsDeanApproved { get; set; }=false;
    }
}