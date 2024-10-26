using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ViewModel
{
    public class ReportViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalForms { get; set; }
        public int EntranceFormsCount { get; set; }
        public int FirstSemExamFormsCount { get; set; }
        public int TotalNotes { get; set; }
        public int TotalNotices { get; set; }
    }

}
