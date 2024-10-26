

namespace Project.Models
{
    public class Semester
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Semester 1", "Semester 2"
        public ICollection<Subject> Subjects { get; set; }
      
    }
}
