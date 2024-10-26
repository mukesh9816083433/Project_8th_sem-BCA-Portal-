using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string? Name { get; set; } // e.g., "Mathematics", "Physics"
        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }
        public ICollection<Note>? Notes { get; set; }
    }
}
