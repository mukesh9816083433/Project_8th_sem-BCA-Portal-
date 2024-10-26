using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Project.Models.ViewModel
{
    public class EightSemExamFormViewModel
    {
        public User? User { get; set; }
        public List<Subject>? EightSemesterSubjects { get; set; }
        public string? Collage { get; set; }
        public int[]? SelectedSubjects { get; set; }
        public IFormFile? PaymentImage { get; set; }

        public bool CollageApproved { get; set; } // Indicates if the form is approved by the collage
        public bool DeanApproved { get; set; }
        public string? PaymentImagePath { get; set; }
    }
}
