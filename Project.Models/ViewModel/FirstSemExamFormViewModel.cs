using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Project.Models;
namespace Project.Models.ViewModel
{
    public class FirstSemExamFormViewModel
    {
        public User? User { get; set; }
        public List<Subject>? FirstSemesterSubjects { get; set; }
        public string? Collagee { get; set; }
        public int[]? SelectedSubjects { get; set; }
        public IFormFile? PaymentImage { get; set; }

        public bool CollageApproved { get; set; } // Indicates if the form is approved by the collage
        public bool DeanApproved { get; set; }
        public string? PaymentImagePath { get; set; }
    }
}
