using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string? Heading { get; set; }
        public string ?Message { get; set; }
        public string? SelectedSubjects { get; set; }
        public List<Semester>? Semesters { get; set; } // Store selected semesters
        public string? ImagePath { get; set; } // Store the path of uploaded image
        public string ? NoticeBy { get; set; } // "By Dean" or "By College"
        public DateTime CreatedAt { get; set; }
         public bool IsAllSemesters { get; set; }
        public ICollection<Semester>? Semester { get; set; }
    }

}
