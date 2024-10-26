using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class FirstSemExamForm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public string? SelectedSubjects { get; set; } // Comma-separated list of subject IDs

        public string? PaymentImagePath { get; set; }
        public DateTime SubmittedAt { get; set; }

        public string? Collage { get; set; }
        public bool CollageApproved { get; set; }
        public bool DeanApproved { get; set; }
    }
}
