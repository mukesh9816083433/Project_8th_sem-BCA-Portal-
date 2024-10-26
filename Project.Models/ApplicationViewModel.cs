using Microsoft.AspNetCore.Http;

namespace Project.Models
{
    public class ApplicationViewModel
    {
        public int userId { get; set; }
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public int[]? SelectedSubjects { get; set; }
        public string? College { get; set; }
        public int Semester { get; set; }
        public string? PaymentImage { get; set; }
        public string? UserName { get; set; }
        public string Status { get; set; }

        public bool Status2 { get; set; }
        public bool Status1 { get; set; }
    }
}
