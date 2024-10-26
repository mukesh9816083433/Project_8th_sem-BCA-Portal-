using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ViewModel
{
    public class NoticeViewModel
    {
        public int Id { get; set; }
        public string? Heading { get; set; }
        public string? Message { get; set; }
        public String? SelectedSubjects { get; set; }
        public List<Semester>? SelectedSems { get; set; }
        public bool IsAllSemesters { get; set; }
        public IFormFile? NoticeImage { get; set; }
        public string? NoticeBy { get; set; } // "By Dean" or "By College"
        public List<Notice> AllNotices { get; set; } // List of notices for the table
        public List<Semester> AllSemesters { get; set; }
        public string ImagePath { get; set; }
        public string? DisplaySemesters { get; set; }
    }

}
